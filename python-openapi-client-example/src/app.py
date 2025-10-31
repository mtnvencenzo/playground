# from app_settings import settings
from cocktails_api.cocktails_api_client.client import Client
from cocktails_api.cocktails_api_client.models.cocktail_rs import CocktailRs
from app_settings import settings

api_client = Client(base_url=settings.api_base_url)

# ...existing code...

from cocktails_api.cocktails_api_client.api.cocktails.get_cocktail import sync_detailed

# Example: Retrieve a cocktail by ID
response = sync_detailed(
    client=api_client,
    id="pegu-club",
    x_key=settings.api_key
)

if response.status_code == 200:
    cocktail_data = response.parsed

    if (cocktail_data is not None) and isinstance(cocktail_data, CocktailRs):
        print("Cocktail retrieved successfully:")
        print("ID:", cocktail_data.item.id)
        print("Title:", cocktail_data.item.descriptive_title)

else:
    print("Failed to retrieve cocktail:", response.status_code)