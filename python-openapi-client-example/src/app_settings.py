import os

from pydantic import Field
from pydantic_settings import BaseSettings, SettingsConfigDict

class AppSettings(BaseSettings):
    model_config = SettingsConfigDict(
        env_file=('.env', f'.env.{os.environ.get("ENV")}'),
        env_file_encoding="utf-8"
    )

    api_base_url: str = Field(
        default="",
        validation_alias='API_BASE_URL'
    )
    api_key: str = Field(
        default="",
        validation_alias='API_KEY'
    )

settings = AppSettings()

# Validate required configuration
if not settings.api_base_url:
    raise ValueError("API_BASE_URL environment variable is required")
if not settings.api_key:
    raise ValueError("API_KEY environment variable is required")

print("App settings loaded successfully.")