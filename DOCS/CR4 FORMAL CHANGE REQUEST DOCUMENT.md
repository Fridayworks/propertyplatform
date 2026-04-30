# CR4 FORMAL CHANGE REQUEST DOCUMENT

FORMAL CHANGE REQUEST DOCUMENT
CHANGE REQUEST NO. CR4
AI AREA RESEARCH ASSISTANT + PDF REPORT DELIVERY MODULE
Project Name: Property Platform
Client: Product Owner / Annie Rustic Property Platform
Vendor: Fridaywork Software House
Document Type: Formal Change Request Submission
CR Reference: CR4
Date: 24 April 2026
1. CHANGE REQUEST TITLE
Implementation of AI Area Research Assistant with Asynchronous PDF
Report Generation and Agent Inbox Delivery
2. CHANGE REQUEST BACKGROUND
During project review of the current platform roadmap, it was identified that although the
platform currently supports:
● listing management,
● AI listing generation,
● CRM lead management,
● analytics and dashboard notifications,
the platform does not yet provide:
a professional research intelligence assistant that enables agents to generate
data-backed area reports for customer consultation.
Property agents frequently require fast access to neighborhood intelligence, local amenities,
investment outlook, transaction data, environmental factors, and lifestyle information in order to
respond to buyer questions professionally.

At present this activity is manual and fragmented across multiple external sources.
Therefore the client has requested a new AI-assisted report request workflow whereby agents
may request an area intelligence report and receive a downloadable PDF generated through an
external AI research API provider.
3. PURPOSE OF THIS CHANGE REQUEST
The purpose of CR4 is to enhance the platform by introducing:
a self-service Area Research module within Agent Dashboard that allows agents to
submit area research queries and receive generated PDF reports delivered via the
existing dashboard inbox.
This enhancement aims to:
● improve agent professionalism,
● shorten customer response time,
● provide knowledge augmentation,
● create future premium monetization opportunities.
4. DETAILED SCOPE OF CHANGE
4.1 New Dashboard Module – AI Area Research
A new menu shall be introduced in Agent Dashboard named:
AI Area Research
This module shall allow agent users to:
● select a township / project / address / custom location,
● optionally pin or define the research area,
● submit one or more free-text questions,
● request full report generation.

4.2 Research Request Job Submission
Upon submission, the platform backend shall:
● create a new research request record,
● assign a processing job ID,
● transmit request payload to external AI Research API provider,
● track processing status asynchronously.
4.3 Asynchronous Job Queue & Polling Mechanism
Because report generation is not immediate, the platform shall implement:
● pending queue management,
● external job polling,
● retry logic,
● failure capture,
● completion callback handling.
Statuses required:
● Pending
● Processing
● Completed
● Failed
● Expired
4.4 Research Report Storage
Upon completion, generated report files shall be:
● stored in secured document storage,
● indexed under requesting Agent account,
● retrievable through request history.
Stored metadata includes:
● Request Number
● Agent ID
● Location Reference

● Submitted Questions
● External Job ID
● Generated File URL
● Report Status
● Completion Timestamp
4.5 Existing Inbox Reuse & Enhancement
The platform currently contains an existing agent inbox / message receiving center under prior
baseline scope.
CR4 shall not create a separate messaging center.
Instead:
the existing inbox infrastructure shall be enhanced to support AI report delivery
notifications.
A new message classification shall be introduced:
AI_REPORT
Upon report completion, the inbox shall display:
● PDF document icon,
● report title,
● generation completed timestamp,
● report download CTA.
This ensures:
● unified notification center,
● no duplicate inbox logic,
● lower engineering complexity.
4.6 Downloadable PDF Report Delivery
Agent users shall be able to:
● open inbox notification,
● view report metadata,

● download generated PDF report.
The platform shall support two external API integration possibilities:
Scenario A — External API returns structured JSON findings
Platform assembles branded PDF internally.
Scenario B — External API returns final PDF file
Platform stores and delivers file directly.
System architecture must support both.
4.7 My Research Requests History Screen
A new request history page shall be provided to display:
● all submitted requests,
● status,
● generated date,
● report availability,
● re-download access.
5. TECHNICAL DEVELOPMENT IMPACT
This CR introduces a new backend service layer:
Research Orchestration Service
with the following technical responsibilities:
● request intake,
● external API communication,
● asynchronous polling,
● result receiving,
● document persistence,
● inbox notification trigger.
This service shall integrate with the existing:

● Dashboard module,
● Inbox module,
● Authentication,
● File storage,
● Notification framework.
Also aligns with current AI orchestration baseline established in Phase 6.
6. DATABASE CHANGE IMPACT
The following new persistent objects are required:
New Tables
● AreaResearchRequest
● AreaResearchReportLog
Existing Table Enhancements
● InboxMessage (new message type support)
● Notification attachment metadata support
Additional storage bucket/folder required for PDF report persistence.
7. API DEVELOPMENT REQUIREMENTS
The following new APIs are required:
POST /api/research/submit
GET /api/research/my-requests
GET /api/research/status/{id}
GET /api/research/download/{id}
Internal service-to-service orchestration endpoints may also be required for job polling.

8. UI/UX DEVELOPMENT REQUIREMENTS
New Screens
● AI Area Research Submission Page
● My Research Requests Page
Existing Screen Enhancement
● Agent Inbox Message Card Enhancement
New UI Components
● PDF report icon card
● processing status chip
● download CTA button
● request progress indicator
9. OUT OF CURRENT CHANGE SCOPE /
EXCLUSIONS
The following are explicitly excluded from this CR quotation:
1. Development of the actual AI intelligence engine.
2. Licensing or subscription fees of third-party data providers.
3. External API provider service charges.
4. Custom authoring of intelligence content if provider does not supply machine-readable
output.
This CR only covers:
platform integration shell, orchestration, storage, delivery, and UI implementation.
10. ASSUMPTIONS
This CR assumes:

● external AI provider API documentation will be supplied by client at later stage,
● provider returns either downloadable PDF or structured report payload,
● provider supports async job status checking,
● file storage capacity is available in deployment environment.
11. ESTIMATED DEVELOPMENT EFFORT
Component Estimated Effort
Dashboard UI Development 3–4 days
Request History UI 2 days
Backend Orchestration APIs 4–5 days
Async Polling / Queue Logic 3–4 days
Inbox Integration Enhancement 2–3 days
File Storage / Download Layer 2 days
QA / UAT / Stabilization 3–5 days
Total Estimated Effort
18 – 25 man days
12. COMMERCIAL CLASSIFICATION
Major Functional Expansion Change Request
Reason:
● introduces new bounded context,
● new service orchestration,
● new dashboard modules,
● external API dependency model,
● document delivery subsystem.

13. BUSINESS BENEFITS TO CLIENT
Upon completion, client platform gains:
● AI intelligence differentiation,
● agent consultative support tool,
● premium feature monetization capability,
● stronger agent retention,
● higher perceived enterprise value.
14. ACCEPTANCE CRITERIA
CR4 shall be deemed accepted when:
● agent can submit research request,
● request is persisted and queued,
● external API orchestration executes,
● completed report appears in request history,
● inbox receives AI report notification,
● PDF file can be downloaded successfully.
15. CHANGE REQUEST STATUS
Pending Client Approval
16. SIGN OFF
Prepared By Fridaywork Software House
Requested By Client / Product Owner
Approval Status Pending

