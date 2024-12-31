from setuptools import setup, find_packages

setup(
    name="profiler",
    version="1.0.0",
    packages=find_packages(),
    install_requires=[
        "fastapi",
        "uvicorn",
        "httpx",
        "pytest",
        "numpy",
        "tensorflow",
        "python-dotenv"
    ],
)
