FROM node:22.16.0-alpine AS build-stage

WORKDIR /app

COPY ../apps/messenger ./apps/messenger
COPY ../packages/ui-kit ./packages/ui-kit
COPY ../package.json ./package.json
COPY ../turbo.json ./turbo.json

COPY . .
RUN npm install
RUN npm run build-messenger

FROM build-stage AS prod-stage

WORKDIR /app

COPY --from=build-stage /app/apps/messenger/.output ./.output/

CMD [ "node", ".output/server/index.mjs" ]
