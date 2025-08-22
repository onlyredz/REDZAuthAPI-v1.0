using Microsoft.AspNetCore.Mvc;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("apistatus")]
    public class StatusPageController : ControllerBase
    {
        [HttpGet]
        public ContentResult Get()
        {
            string authStatus = "online";
            string licenseStatus = "online";
            string dbStatus = "online";

            string GetStatusClass(string status) =>
                status switch
                {
                    "online" => "status-ok",
                    "warning" => "status-warning",
                    "offline" => "status-error",
                    _ => "status-unknown"
                };

            var html = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <title>Status - REDZAuth API</title>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <style>
        :root {{
            --bg: #1f1f1f;
            --panel: #2a2a2a;
            --text: #e0e0e0;
            --primary: #00ffb3;
            --ok: #3fe07f;
            --warning: #ffcc00;
            --error: #ff4d4d;
            --font: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }}

        body {{
            margin: 0;
            background: var(--bg);
            color: var(--text);
            font-family: var(--font);
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            animation: fadeIn 1s ease;
        }}

        .container {{
            background: var(--panel);
            padding: 40px 50px;
            border-radius: 16px;
            max-width: 600px;
            width: 100%;
            box-shadow: 0 0 20px rgba(0,0,0,0.6);
        }}

        h1 {{
            font-size: 2.2rem;
            color: var(--primary);
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 10px;
        }}

        .status-dot {{
            width: 14px;
            height: 14px;
            border-radius: 50%;
            animation: pulse 2s infinite;
        }}

        .status-ok {{ background: var(--ok); }}
        .status-warning {{ background: var(--warning); }}
        .status-error {{ background: var(--error); }}

        ul {{
            list-style: none;
            padding: 0;
            margin-top: 30px;
        }}

        li {{
            background: #333;
            border-radius: 10px;
            margin-bottom: 15px;
            padding: 15px 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-weight: 500;
            box-shadow: inset 0 0 5px rgba(0, 0, 0, 0.3);
        }}

        .tag {{
            padding: 6px 14px;
            border-radius: 30px;
            font-size: 0.9rem;
            font-weight: 600;
            text-transform: uppercase;
        }}

        .tag-ok {{
            background-color: var(--ok);
            color: #111;
        }}

        .tag-warning {{
            background-color: var(--warning);
            color: #111;
        }}

        .tag-error {{
            background-color: var(--error);
            color: #111;
        }}

        footer {{
            margin-top: 30px;
            text-align: center;
            font-size: 0.9rem;
            color: #888;
        }}

        @keyframes pulse {{
            0%, 100% {{ transform: scale(1); opacity: 1; }}
            50% {{ transform: scale(1.2); opacity: 0.6; }}
        }}

        @keyframes fadeIn {{
            from {{ opacity: 0; transform: translateY(10px); }}
            to {{ opacity: 1; transform: translateY(0); }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>
            <div class='status-dot {GetStatusClass("online")}'></div>
            REDZAuth API
        </h1>
        <p>Overall Status: <strong>API ONLINE</strong></p>

        <ul>
            <li>
                Authentication
                <span class='tag tag-{authStatus}'>
                    {authStatus.ToUpper()}
                </span>
            </li>
            <li>
                License Keys
                <span class='tag tag-{licenseStatus}'>
                    {licenseStatus.ToUpper()}
                </span>
            </li>
            <li>
                Database
                <span class='tag tag-{dbStatus}'>
                    {dbStatus.ToUpper()}
                </span>
            </li>
        </ul>

        <footer>© 2024 - {DateTime.Now.Year} REDZAuth API | All rights reserved.</footer>
    </div>
</body>
</html>";

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };
        }
    }
}
