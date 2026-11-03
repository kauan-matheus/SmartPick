pipeline {
    agent any

    options {
        timestamps()
        disableConcurrentBuilds()
        buildDiscarder(logRotator(numToKeepStr: '15'))
    }

    environment {
        TAG = "${env.GIT_COMMIT.take(7)}"
        DEPLOY_ENV = "${env.BRANCH_NAME == 'main' ? 'prod' : 'homolog'}"
        DEPLOY_DIR = '/opt/smartpick'
        DEPLOY_USER = 'ansible'
        IMAGES = "smartpick/api:${TAG} smartpick/pathfinder:${TAG} smartpick/frontend:${TAG}"
    }

    stages {
        stage('Testes') {
            parallel {
                stage('PathFinder') {
                    steps {
                        sh 'docker build --target test backend/PathFinderService'
                    }
                }
                stage('Frontend') {
                    steps {
                        sh 'docker build --target lint frontend'
                    }
                }
            }
        }

        stage('Build') {
            steps {
                withCredentials([file(credentialsId: "smartpick-env-${DEPLOY_ENV}", variable: 'ENV_FILE')]) {
                    sh 'cp "$ENV_FILE" .env'
                }
                sh 'docker compose build'
            }
        }

        stage('Deploy') {
            when { anyOf { branch 'main'; branch 'homolog' } }
            steps {
                withCredentials([string(credentialsId: "deploy-host-${DEPLOY_ENV}", variable: 'HOST')]) {
                    sshagent(credentials: ['deploy-ssh-key']) {
                        sh '''
                            SSH="ssh -o StrictHostKeyChecking=accept-new $DEPLOY_USER@$HOST"
                            docker save $IMAGES | gzip | $SSH "gunzip | docker load"
                            scp -o StrictHostKeyChecking=accept-new docker-compose.yml .env $DEPLOY_USER@$HOST:$DEPLOY_DIR/
                            $SSH "cd $DEPLOY_DIR && TAG=$TAG docker compose up -d --no-build --remove-orphans"
                        '''
                    }
                }
            }
        }

        stage('Verificação') {
            when { anyOf { branch 'main'; branch 'homolog' } }
            steps {
                withCredentials([string(credentialsId: "deploy-host-${DEPLOY_ENV}", variable: 'HOST')]) {
                    sshagent(credentials: ['deploy-ssh-key']) {
                        sh '''
                            SSH="ssh -o StrictHostKeyChecking=accept-new $DEPLOY_USER@$HOST"
                            for i in $(seq 1 30); do
                                if [ "$($SSH "cd $DEPLOY_DIR && docker compose ps --format '{{.Health}}' | grep -cx healthy")" -ge 4 ]; then
                                    exit 0
                                fi
                                sleep 5
                            done
                            $SSH "cd $DEPLOY_DIR && docker compose ps && docker compose logs --tail 30"
                            exit 1
                        '''
                    }
                }
            }
        }
    }

    post {
        always {
            sh 'rm -f .env'
            sh 'docker image prune -f'
        }
    }
}
