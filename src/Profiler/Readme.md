# Profiler API Project

This is a API-based project designed to provide profiling features. The project supports both local development and containerized execution using Docker.

---

## **Installation**

### 1. Set Up a Virtual Environment (Optional)

It's recommended to use a virtual environment for local development.

```bash
python -m venv venv
source venv/bin/activate  # On Windows, use `venv\Scripts\activate`
```

```bash
pip install -r requirements.txt
```

### 2. Install Dependencies

```bash
pip install -r requirements.txt
```

## **Running the Application Locally**

### 1. Start the App

Run the following command to start the server:

```bash
uvicorn app.main:app --reload
```

### 2. Access the Application

Visit http://127.0.0.1:8000 in your browser to access the API.

## Running Tests

Make sure all dependencies are installed, and then run:

```bash
pytest
```

## Using Docker

### 1. Build the Docker Image

```bash
docker build -t profiler-api .
```

### 2. Run the Docker Container

```bash
docker run -d -p 8000:8000 --name profiler-api profiler-api
```

### 3. Access the Application

Visit http://127.0.0.1:8000 in your browser to access the API.

## **Debugging the Application**

If you need to debug the application, you can run the container in interactive mode:

```bash
docker run -it --rm -p 8000:8000 --name profiler-api profiler-api /bin/bash
```

From inside the container, you can run commands or start the app manually:

```bash
uvicorn app.main:app --host 0.0.0.0 --port 8000
```
