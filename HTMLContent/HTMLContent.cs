namespace CarWebsiteBackend.HTMLContent
{
    public static class HTMLContent
    {
        public static string emailBody(string link) 
        {
            string emailBody = @"
            <html>
                <head>
                    <title>Royal Motors - Email Verification</title>
                    <style>
                        body {
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                            font-size: 16px;
                            color: #444444;
                            }
                        .header {
                            background-color: #0d47a1;
                            color: #ffffff;
                            padding: 20px;
                            }
                        .header h1 {
                            margin: 0;
                            font-size: 28px;
                            }
                        .button {
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #4caf50;
                            color: #ffffff;
                            text-decoration: none;
                            border-radius: 5px;
                            margin-top: 20px;
                            }
                        .button:hover {
                            background-color: #388e3c;
                        }
                        .footer {
                            background-color: #f2f2f2;
                            padding: 20px;
                            text-align: center;
                            font-size: 14px;
                            }
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>Royal Motors</h1>
                    </div>
                    <div class='content'>
                        <h2 style='background-color: #0d47a1; color: #ffffff; padding: 10px;'>Verify your email address</h2>
                        <p>Please click the button below to verify your email address:</p>
                        <form method='post' action='" + link + @"'>
                            <button type='submit' class='button'>Verify Email</button>
                        </form>
                    </div>
                        <div class='footer'>
                        <p>This email was sent by Royal Motors, located at Dahye, Beirut.</p>
                    </div>
                </body>
            </html>";
            return emailBody;
        }

        public static string verifySuccessWebsite() 
        {
            string html = @"
        <html>
            <head>
	                <title>Email Verified</title>
            </head>
            <body>
	                <h1>Email Verified</h1>
	                <div style=""color: green;"">
		                <svg xmlns=""http://www.w3.org/2000/svg"" width=""50"" height=""50"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""feather feather-check-circle"">
  			            <path d=""M22 11.08V12a10 10 0 1 1-5.93-9.14""></path>
  			            <polyline points=""22 4 12 14.01 9 11.01""></polyline>
		                </svg>
		                <span style=""vertical-align: middle;"">Your email has been successfully verified.</span>
	                </div>
            </body>
        </html>";
            return html;
        }

        public static string verifyFailWebsite()
        {
            string html = @"
        <html>
            <head>
                <title>Email Verified</title>
            </head>
            <body>
                <h1>Email Verification Failed</h1>
                <div style='color: red;'>
                    <svg xmlns='http://www.w3.org/2000/svg' width='50' height='50' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' class='feather feather-x-circle'>
                        <circle cx='12' cy='12' r='10'></circle>
                        <line x1='15' y1='9' x2='9' y2='15'></line>
                        <line x1='9' y1='9' x2='15' y2='15'></line>
                    </svg>
                    <span style='vertical-align: middle;'>Sorry, we could not verify your email address.</span>
                </div>
            </body>
        </html>";
            return html;
        }

        public static string alreadyVerifiedWebsite()
        {
            string html = @"
        <html>
            <head>
                <title>Email Verified</title>
            </head>
            <body>
                <h1>Email Verification Failed</h1>
                <div style='color: red;'>
                    <svg xmlns='http://www.w3.org/2000/svg' width='50' height='50' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' class='feather feather-x-circle'>
                        <circle cx='12' cy='12' r='10'></circle>
                        <line x1='15' y1='9' x2='9' y2='15'></line>
                        <line x1='9' y1='9' x2='15' y2='15'></line>
                    </svg>
                    <span style='vertical-align: middle;'>Account already verified.</span>
                </div>
            </body>
        </html>";
            return html;
        }
    }
}
