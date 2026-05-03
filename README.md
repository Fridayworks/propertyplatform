# PropertyPlatform

PropertyPlatform is a multi-tenant SaaS marketplace for real estate listings that connects property agents and buyers through a high-performance, search-first experience.

## Features

- Property listing CRUD
- Search and filtering
- AI recommendations
- Lead generation and tracking
- Agent onboarding
- Referral system
- Monetization system

## AI Smart Listing Generation Engine

This platform implements an AI Smart Listing Generation Engine that automates the creation of listing content to improve productivity for agents.

### Implemented Features

- **AI Selling Point Extraction**: Automatically generates compelling selling points based on property features
- **AI SEO Title Generation**: Creates optimized SEO titles for better search visibility
- **AI Description Generation**: Generates engaging property descriptions
- **AI FAQ Generation**: Creates relevant FAQ sections for listings
- **AI Nearby Amenity Narrative**: Provides narrative descriptions of nearby amenities
- **AI Editable Draft Composer**: Creates a complete editable draft for agents to review and modify

### API Endpoints

The AI Smart Listing Generation Engine exposes the following endpoints:

- `POST /api/smartlisting/generate-selling-points`
- `POST /api/smartlisting/generate-seo-title`
- `POST /api/smartlisting/generate-description`
- `POST /api/smartlisting/generate-faq`
- `POST /api/smartlisting/generate-nearby-amenities`
- `POST /api/smartlisting/generate-draft`

Each endpoint accepts a `ListingId` in the request body and returns AI-generated content for that specific listing.