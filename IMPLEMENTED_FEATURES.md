

# PropertyPlatform - Implemented Features

## Overview
PropertyPlatform is a comprehensive real estate platform that enables agents to list properties, manage leads, and interact with clients. The platform includes both API endpoints and a web UI for agent management.

## API Features

### Authentication
- User registration with email/password
- JWT-based authentication
- Refresh token support for secure sessions
- Admin role management

### Property Listings
- Create, read, update, and delete property listings
- Support for multiple listing types (Sale, Rent, etc.)
- Property type categorization (Condo, Terrace, Semi-D, Landed, etc.)
- Listing status management (Draft, Active, etc.)
- Property media management (images, floor plans)
- Property features management (bedrooms, bathrooms, etc.)

### Projects & Developers
- Create, read, update, and delete projects
- Project entity with developer information, location, and project details
- Developer entity with company information and contact details
- UnitType entity with specifications and pricing for different unit types within projects
- Project media management (images, PDFs, etc.)
- Bulk project media processing

### Leads Management
- Lead capture from property listings
- Lead scoring system
- Lead retrieval for agents
- Lead tracking and management

### Featured Listings
- Feature listing boost functionality
- Boost level management (standard, premium, top)
- Listing duration management (7 days by default)
- Credit-based system for featuring listings

### Search & Recommendations
- Advanced property search with filters (keyword, location, price, type)
- Recommendation engine for similar properties
- Pagination support for search results

### User Management
- Agent profile management
- Credit system for featured listings
- Subscription management
- Referral system for user acquisition

### Admin Configuration
- Feature flag management
- Configuration updates for platform features
- Admin-only access controls

### Brochure Ingestion
- PDF/URL/image ingestion
- OCR extraction from documents
- Bulk project media processing
- Automated content parsing

## Web UI Features

### Agent Dashboard
- Agent landing page with overview
- Property listing management
- Lead management interface
- Profile editing
- Subscription management
- Wallet/credits management

### Property Management
- Create new property listings
- Edit existing property listings
- View property details
- Upload property media and floor plans
- Manage property features

### User Experience
- Responsive design for all devices
- Property search functionality
- Property comparison tools
- Lead tracking and management
- Agent profile and bio management
- Subscription and payment management

### Additional Features
- Referral system for user acquisition
- Credit-based system for premium features
- User events tracking
- Property analytics (views, clicks, contacts)
- Project and developer management
- Brochure ingestion interface

## Data Model

### Core Entities
- **PropertyListing**: Main property listing entity with title, description, price, location, etc.
- **AgentProfile**: Agent profile with credentials, credits, and experience points
- **Lead**: Customer lead information with contact details and score
- **FeaturedListing**: Featured property listing with boost level and duration
- **PropertyMedia**: Property images and media
- **PropertyFeature**: Property features (bedrooms, bathrooms, etc.)
- **FloorPlan**: Floor plan images for properties
- **ListingAnalytic**: Analytics data for property listings
- **UserEvent**: User interaction tracking
- **Subscription**: User subscription management
- **Referral**: Referral tracking between users
- **Tenant**: User account information
- **RefreshToken**: Token management for authentication
- **Project**: Project entity with developer information, location, and project details
- **Developer**: Developer entity with company information and contact details
- **UnitType**: Unit type entity with specifications and pricing for different unit types within projects
- **ProjectMedia**: Media files related to projects (images, PDFs, etc.)
- **UnitTypeMedia**: Media files related to specific unit types

### Database
- PostgreSQL database with Entity Framework Core
- Migration support for database schema changes
- Relationship mapping between entities

## Technical Implementation

### Architecture
- .NET 10 Web API backend
- ASP.NET Core MVC for web UI
- Entity Framework Core for data access
- JWT authentication
- PostgreSQL database

### Security
- JWT-based authentication
- Role-based access control
- Password hashing with BCrypt
- Secure token management

### Features
- Credit-based system for premium features
- Subscription management
- Referral system
- Recommendation engine
- Analytics tracking
- Project and developer management
- Brochure ingestion and OCR processing