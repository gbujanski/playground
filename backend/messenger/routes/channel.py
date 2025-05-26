from fastapi import (
    APIRouter,
    Depends 
)
from db.database import get_session
from db.models.channel import Channel
from sqlalchemy.orm import Session

from services.channel import ChannelService

channel_router = APIRouter()

@channel_router.post("/channel")
def create_channel(
    channel: Channel,
    session: Session = Depends(get_session)
) -> Channel:
    return ChannelService.create_channel(channel, session)


@channel_router.get("/channel")
def get_all_channels(
    session: Session = Depends(get_session)
) -> list[Channel]:
    return ChannelService.get_all_channels(session)


@channel_router.delete("/channel/{channel_id}")
def delete_channel(
    channel_id: int,
    session: Session = Depends(get_session)
) -> None:
    ChannelService.delete_channel(channel_id, session)
