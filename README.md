# BookApp

Responsive CRUD app with JWT auth: a shared **Books** catalogue and per-user **Quotes**.

- **Live app:** https://bookapp-test.netlify.app
- **API:** https://book-app-kxki.onrender.com — OpenAPI UI at `/scalar`, health at `/health`
- **Demo account:** `testuser` / `test12345`

> The API runs on Render's free tier. If it has been idle, the first request can take
> ~50 seconds while the instance wakes up. Everything after that is fast.

## Stack

| Layer    | Choice                                                                |
| -------- | --------------------------------------------------------------------- |
| Frontend | Angular 20, standalone components, signals, Bootstrap 5, Font Awesome |
| Backend  | .NET 9, ASP.NET Core controllers, EF Core                             |
| Database | PostgreSQL (Neon)                                                     |
| Auth     | JWT bearer, `PasswordHasher<User>`                                    |
| Hosting  | Netlify (web), Render (API), Neon (database)                          |

## Running locally

Requires the .NET 9 SDK (pinned in `global.json`), Node 22+, and a PostgreSQL connection string.

```bash
# API — http://localhost:5169
cd api
dotnet user-secrets set "ConnectionStrings:Default" "<postgres connection string>" \
  --project src/BookApp.Api
dotnet user-secrets set "Jwt:Key" "<32+ character signing key>" --project src/BookApp.Api
dotnet run --project src/BookApp.Api
```

```bash
# Web — http://localhost:4200
cd web
npm ci
npm start
```

Only those two are secrets. `Cors:AllowedOrigin`, the JWT issuer, audience, and expiry
ship in `appsettings.json` with working local defaults.

Migrations run on startup, so the first launch creates the schema and seeds a demo user
with five quotes. No manual `dotnet ef database update` step.

### Running on the local network

To open the app from a phone or another machine on the same Wi-Fi, both servers have to
listen on the network interface rather than on loopback, and the API has to allow the
new origin.

```bash
# API — binds 0.0.0.0:5169 via the "lan" launch profile
cd api
dotnet run --project src/BookApp.Api --launch-profile lan
```

```bash
# Web — npm start already passes --host 0.0.0.0
cd web
npm start
```

Find your address with `ipconfig getifaddr en0` (macOS) or `hostname -I` (Linux), then
add that origin to `Cors:AllowedOrigin` in `appsettings.Development.json` — the value is
a comma-separated list, so keep localhost alongside it:

```json
"Cors": { "AllowedOrigin": "http://localhost:4200,http://192.168.0.80:4200" }
```

Then browse to `http://192.168.0.80:4200`. The frontend derives its API URL from
`location.hostname` in development, so it calls the same host it was served from — no
rebuild and no hardcoded IP in a committed file.

## With more time

- Refresh-token flow with the access token held in memory
- Pagination and search on the books list — it currently returns every row
- Optimistic concurrency
- Angular component tests around the forms, and more API tests than the four here
