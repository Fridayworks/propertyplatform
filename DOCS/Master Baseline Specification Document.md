# Master Baseline Specification Document

ANNIE RUSTIC PROPERTY PLATFORM
Master Baseline Specification Document
Version 2.0
Document Classification: Board / Stakeholder Master Blueprint
Prepared For: Annie Rustic Property Platform Ownership
Prepared By: Product Engineering Consolidation
Document Purpose: Unified governing specification consolidating all approved platform
phases and change requests.
Revision History
Version Date Description
1.0 Initial Baseline Core platform concept
1.5 Phase 5 Agent-centric SaaS marketplace extension
1.6 Phase 6 AI smart listing engine extension
2.0 Current Consolidated CR1, CR2, CR3, CR5 master
baseline
Table of Contents
1. Executive Product Vision
2. Consolidated Business Objectives
3. Final Approved Module Landscape
4. Master Functional Requirements Baseline
5. Master Non-Functional Requirements
6. Master Architecture Baseline
7. Master Data Baseline
8. Master API Baseline
9. Master Monetization Baseline

10. Master Delivery Classification
11. Master Risk Baseline
12. Version Control Declaration
1. Executive Product Vision
Annie Rustic Property Platform shall operate as an AI-powered Agent-Centric Property
Operating System combining:
● property marketplace,
● agent branded microsites,
● AI listing automation,
● CRM lead management,
● wallet monetization,
● enterprise governance,
● CMS content marketing,
● platform-wide analytics intelligence.
This platform is no longer a normal listing website.
It is a:
SaaS + AI + Marketplace + Governance Hybrid Platform
2. Consolidated Business Objectives
2.1 Agent Digital Ownership
Allow agents to own:
● landing pages,
● custom domains,
● branding,
● SEO identity.
2.2 AI Productivity
Allow agents to rapidly create property and project listings using AI ingestion.
2.3 Lead Conversion

Provide CRM and lead nurturing tools.
2.4 Platform Monetization
Generate revenue via subscriptions, credits, boosts, and premium domains.
2.5 Enterprise Governance
Allow owner full operational governance through Super Admin Control Tower.
2.6 Content Marketing & SEO
Allow owner to publish article/news and dynamic website campaigns.
3. Final Approved Module Landscape
3.1 Core Agent SaaS Modules
● Agent Registration & Authentication
● Agent Profile Management
● Agent Landing Page Builder
● Agent Dashboard
● Agent CRM
● Agent Gamification
● Agent Wallet
3.2 Marketplace Modules
● Property Listing Management
● Property Search & Discovery
● Ranking Engine
● Boost Campaign Engine
● Leads Capture
3.3 AI Smart Listing Modules
● Project Ingestion Engine
● PDF/URL/Image OCR Processor
● AI Data Extraction
● AI Listing Description Generator
● AI Theme Generator
● Unit Layout Visualizer

3.4 Monetization Modules
● Subscription Plans
● Credit Wallet
● Credit Ledger
● Domain Purchase
● Featured Visibility Purchase
3.5 Governance Modules
● Super Admin Portal
● User Governance
● Role Permission Matrix
● Site Configuration Center
● CMS Articles/News
● Dynamic Menu Builder
● Theme Governance
● Global Analytics Dashboard
● Admin Audit Logs
3.6 Shared Infrastructure Modules
● Notification Engine
● Media Service
● Payment Gateway Service
● Analytics Aggregator
● Event Bus
4. Master Functional Requirements Baseline
The platform shall support:
● Agent registration/login/profile/branding
● Agent custom landing pages
● Manual + AI property listing creation
● AI project ingestion from PDF/URL/images
● Lead capture and CRM notes/status
● Credit purchase and wallet ledger
● Agent ranking and boosts
● Super admin governance and user management
● CMS article/news publishing
● Dynamic menus and global theme switching

● Global analytics and revenue monitoring
5. Master Non-Functional Requirements
● Modular microservice architecture
● Docker deployment ready
● APISIX routed APIs
● Event driven async processing
● JWT + RBAC security
● Immutable audit logs
● Horizontal scalability
● CDN ready static delivery
● <3 sec frontend load
6. Master Architecture Baseline
Users / Buyers / Agents / Super Admin
→ APISIX Gateway
→ Agent / Listing / AI / Wallet / Boost / CRM / CMS / Admin Services
→ Event Bus / Analytics Queue
→ Database + Redis + Media Storage
7. Master Data Baseline
Identity Domain
● Agents
● AdminUsers
● Roles
● Permissions
Listing Domain
● Projects
● UnitTypes
● Listings
● Media

Monetization Domain
● Wallet
● CreditTransaction
● Payment
● BoostCampaign
● Subscription
CRM Domain
● Leads
● LeadNotes
Governance Domain
● SiteSettings
● Articles
● Menus
● Themes
● AuditLogs
Intelligence Domain
● ListingAnalytics
● GlobalAnalyticsSnapshots
8. Master API Baseline
Approved API families:
● /api/auth/*
● /api/agents/*
● /api/listings/*
● /api/projects/*
● /api/ai/*
● /api/leads/*
● /api/wallet/*
● /api/boost/*
● /api/dashboard/*
● /api/admin/*
● /api/cms/*
● /api/themes/*

● /api/menus/*
9. Master Monetization Baseline
Subscription Plans:
● Free
● Starter
● Pro
● Elite
Credit Packages:
● 10 credits
● 50 credits
● 100 credits
Revenue Sources:
● Subscription
● Credit consumption
● Domain purchases
● Featured exposure
10. Master Delivery Classification
This platform is officially classified as an:
Enterprise Grade Multi-Phase Product Ecosystem
Development should be managed in milestone streams rather than as a simple website build.
11. Master Risk Baseline
Technical Risks
● AI extraction accuracy
● RBAC security leakage

● Dashboard heavy aggregation
● Theme governance regression
● Wallet financial integrity
Business Risks
● Agent onboarding friction
● Insufficient baseline leads
● Content SEO maintenance
12. Consolidated Approved Change Request Register
CR1 – Core Platform Enhancement Package
Introduced foundational expansion items previously approved under CR1 covering early
platform operational improvements.
CR2 – Additional Platform Service Enhancements
Introduced subsequent service and workflow improvements approved under CR2 for platform
capability strengthening.
CR3 – Advanced Operational and Business Flow Enhancements
Introduced additional business process improvements, user workflow enhancements, and
scalability preparation approved under CR3.
CR5 – Super Admin Governance & CMS Enterprise Control Tower
Introduced enterprise governance modules including:
● Super Admin user management
● Role permission matrix
● Website global settings
● News/article CMS
● Dynamic menu builder
● Theme governance
● Global analytics dashboard
● Audit logging
Consolidation Note

All above approved change requests are deemed absorbed into this Master Baseline Version
2.0 and are no longer treated as isolated enhancement notes. Future development shall
reference this unified baseline.
13. Version Control Declaration
This Version 2.0 document is the governing project baseline.
All future change requests and engineering decisions shall be referenced against this master
document.

