## Run with Docker Compose

Create a root-level `.env` file containing your Warsaw API token (do not commit this file):

```dotenv
WARSAW_API_TOKEN=your-token-here
```

Start the API with `docker compose up --build`. The API is available over HTTP at `http://localhost:8080`.