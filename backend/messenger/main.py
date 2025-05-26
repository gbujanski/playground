from fastapi import (
    FastAPI,
    APIRouter
)
from fastapi.middleware.cors import CORSMiddleware
from db.setup import create_db_and_tables
from routes.channel import channel_router

app = FastAPI()
router = APIRouter(prefix="/api/messenger")

origins = [
    "http://localhost:8000",
    "http://localhost:3000",
    "http://localhost:80",
    "http://localhost",
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

create_db_and_tables()

router.include_router(channel_router)
app.include_router(router)
