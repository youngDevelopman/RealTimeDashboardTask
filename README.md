# RealTimeDashboard

Develop a real-time analytics dashboard for a fictional e-commerce platform. The dashboard should display various metrics such as active users, total sales, and top-selling products, updated in real-time. You will need to create a custom API to fetch this data and integrate it with the frontend.

## Requirements
### Backend (API)
- Create a RESTful API using any backend framework (e.g., C# .NET).
- Implement endpoints to fetch:
  - Active users (simulated data with random values)
  - Total sales (simulated data with random values)
  - Top-selling products (list of products with sales figures)
- Use WebSockets to push updates to the frontend in real-time.
- Include necessary unit tests for the API endpoints.
### Frontend
- Create a single-page application (SPA) using a modern JavaScript framework/library (e.g., React).
- Display the following metrics on the dashboard:
  - Active users (updated every 10 seconds)
  - Total sales (updated every 10 seconds)
  - Top-selling products (updated every 30 seconds)
- Implement real-time updates using WebSockets.
- Use a charting library (e.g., Chart.js, D3.js) to visualize the data.
- Include responsive design to ensure the dashboard is mobile-friendly.
### Data Simulation
- Implement a simple data simulation mechanism in the backend to generate random data for the metrics.
- Ensure the data changes over time to simulate a real-world scenario.


## Tech stack
- ASP.NET CORE
- Angular
- WebSockets
- ChartJs
- xUnit
## Working example of UI
![image](https://github.com/user-attachments/assets/b1223dc8-a70a-4195-9f60-8ba5da338dcf)

![image](https://github.com/user-attachments/assets/571b73e3-8afe-4ba9-b5c7-8e636da6d144)

![image](https://github.com/user-attachments/assets/759c8a9b-681a-40ba-96b8-b2e1f6e6138c)

