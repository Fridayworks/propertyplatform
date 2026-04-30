# Change Request (CR1): Listing Type Selection + Admin Configuration Control

## 1. Change Summary

Introduce a listing type selection layer for agents and a configuration control panel for platform administrators. This enhancement standardizes listing creation flows and enables system-level feature toggling.

---

## 2. Background / Problem Statement

Currently, listing creation lacks structured entry points, resulting in:

- **Inconsistent data input**
- **Poor UX for agents**
- **Limited control for platform administrators**

Agents need a guided workflow, while admins need feature governance.

---

## 3. Proposed Change

### 3.1 Agent-Side: Listing Type Selection Page

Before entering the listing creation flow, agents must first select one of three listing types:

#### Listing Types

1. **Sale** - Standard property sale
2. **Rent** - Residential or commercial rental
3. **New Project** - Developer/project-based listing

#### 3.1.1 UI Behavior

- A pre-entry page with 3 selectable buttons/cards:
  - Sale
  - Rent
  - New Project
- This page acts as a decision gateway before navigating to detailed forms

#### 3.1.2 Functional Behavior

**Option 1: Sale**
- Standard property sale input fields:
  - Price
  - Property details
  - Location
  - Description

**Option 2: Rent**
- Rental-specific fields:
  - Monthly rental price
  - Rental configuration:
    - Whole unit
    - Room rental
    - Shared rental
  - Optional: Split rental configuration module

**Option 3: New Project**
- Developer/project-based listing:
  - Project overview
  - Floor plans
  - Unit types
  - Media (brochure, images)
- Supports richer structured data (aligned with Phase 6 ingestion engine)

### 3.2 Admin-Side: Feature Configuration Panel

Introduce a Super Admin / Platform Admin configuration module.

#### 3.2.1 Core Capability

Admin can enable/disable listing types dynamically:

| Feature | Description |
|---------|-------------|
| Enable Sale | Toggle sale listings |
| Enable Rent | Toggle rental listings |
| Enable New Project | Toggle project listings |

#### 3.2.2 Behavior

- **Disabled options:**
  - Hidden or greyed out in UI
- **System enforces:**
  - API validation (cannot bypass UI)

#### 3.2.3 Future Extensibility

- Feature flags framework
- Region-based enablement
- Subscription-tier restrictions

---

## 4. Functional Requirements

| Requirement | Description |
|-------------|-------------|
| FR-CR1-01 | System shall present a listing type selection page before listing creation. |
| FR-CR1-02 | System shall route users to different forms based on selected type. |
| FR-CR1-03 | System shall support rental configuration options for Rent type. |
| FR-CR1-04 | System shall support enhanced project structure for New Project. |
| FR-CR1-05 | Admin shall be able to enable/disable listing types. |
| FR-CR1-06 | System shall enforce configuration at API and UI level. |

---

## 5. Non-Functional Requirements

- Configuration changes must reflect in real-time (<5 seconds)
- Feature toggles must not require redeployment
- Must support future feature flags architecture

---

## 6. System Impact Analysis

### 6.1 Affected Modules

- Listing Service
- UI (MAUI + Web)
- Admin Panel
- API Gateway
- Database

### 6.2 Database Changes

**New Table: FeatureConfig**

| Column | Type | Description |
|--------|------|-------------|
| Id | PK | Primary Key |
| FeatureKey | string | e.g., ENABLE_SALE |
| IsEnabled | boolean | Toggle state |
| UpdatedAt | datetime | Timestamp |

### 6.3 API Changes

- **GET /api/config/features**
  - Returns enabled features

- **PUT /api/admin/config/features**
  - Update feature toggles

- **POST /api/listings**
  - Must validate listing type against config

### 6.4 UI Changes

- **New Screen:** Listing Type Selection Page
- **Admin Panel:** Feature toggle switches

---

## 7. UX Flow

### Agent Flow

Create Listing -> [Select Type Page] -> (Sale / Rent / New Project) -> Dynamic Form -> Submit Listing

### Admin Flow

Admin Panel -> Configuration Section -> Toggle Features -> Save

---

## 8. Risks & Mitigation

| Risk | Mitigation |
|------|-----------|
| Confusion in UX | Clear labeling + icons |
| Misconfiguration by admin | Default fallback enabled |
| API bypass | Server-side validation |

---

## 9. Dependencies

- Existing listing module
- Authentication (Admin role)
- Config service / caching (Redis recommended)

---

## 10. Priority & Phase

- **Priority:** High
- **Phase:** Phase 6 Extension
- **Type:** UX + Platform Governance Enhancement

---

## 11. Acceptance Criteria

- Agent must see 3 options before creating listing
- Each option loads correct form
- Admin can toggle each option
- Disabled option cannot be used via API or UI

---

## 12. Strategic Impact

This is not just a UI tweak. It introduces:

- **Structured data entry model** - Consistent approach to different listing types
- **Scalable feature control system** - Dynamic feature toggling without redeployment
- **Foundation for future monetization** - Feature gating, tiering, and advanced controls

### Next Steps

- Design MAUI UI wireframe (production-level)
- Generate .NET backend code for feature toggle system
- Extend this into Phase 7 roadmap (feature flag + A/B testing engine)

---

**Document Generated From:** CR1 - Listing Type Selection + Admin Configuration Control.pdf
**Status:** Active Change Request
**Last Updated:** April 22, 2026
