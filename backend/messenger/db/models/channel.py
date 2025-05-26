from sqlalchemy import String
from sqlalchemy.orm import (
    Mapped,
    mapped_column,
    DeclarativeBase
)

class Base(DeclarativeBase):
    pass

class Channel(Base):
    __tablename__ = "channel"
    
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String)
    