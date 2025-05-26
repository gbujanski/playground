from fastapi import HTTPException
from db.models.channel import Channel
from sqlalchemy import select
from sqlalchemy.orm import Session

class ChannelService:
    @staticmethod
    def create_channel(channel: Channel, session: Session) -> Channel:
        session.add(channel)
        session.commit()
        session.refresh(channel)
        return channel
    
    @staticmethod
    def delete_channel(channel_id: int, session: Session) -> None:
        channel = session.get(Channel, channel_id)
        if (not channel):
            raise HTTPException(status_code=404, detail="Channel not found")
        session.delete(channel)
        session.commit()

    @staticmethod
    def get_all_channels(session: Session) -> list[Channel]:
        stmt = select(Channel)
        result = session.exec(stmt)
        return result