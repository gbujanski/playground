# Etap 1: Budowanie aplikacji
FROM node:22.16.0-alpine AS build-stage

WORKDIR /app

COPY ../apps/auth ./apps/auth
COPY ../packages/ui-kit ./packages/ui-kit
COPY ../package.json ./package.json
COPY ../turbo.json ./turbo.json

RUN npm install
RUN npm run build-auth

FROM build-stage AS prod-stage

WORKDIR /app

COPY --from=build-stage app/apps/auth/.output ./.output/

CMD [ "node", ".output/server/index.mjs" ]
