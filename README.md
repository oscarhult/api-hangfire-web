```
docker run \
  --rm \
  -e ASPNETCORE_URLS="http://+:5555" \
  -p 5555:5555 \
  ghcr.io/oscarhult/api-hangfire-web:master
```
