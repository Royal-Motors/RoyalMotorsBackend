using CarWebsiteBackend.Email;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
                        background-color: #1c2f36;
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
                        background-color: #1c2f36;
                        color: #ffffff;
                        text-decoration: none;
                        border-radius: 5px;
                        margin-top: 20px;
                        margin-left: 3rem;
                        }
                    .button:hover {
                        background-color: #95c197;
                    }
                    .footer {
                        background-color: #1c2f36;
                        padding: 20px;
                        text-align: center;
                        font-size: 14px;
                        color: white;
                        }


                        .content p{
                            margin-left: 3rem;
                        }
                </style>

                </head>

                <body>
                    <div class='header'>
                        <h1>Royal Motors</h1>
                    </div>
                    <div class='content'>
                        <h2 style='background-color: #1c2f36; color: #ffffff; padding: 20px;'>Verify your email address</h2>
                        <p>Please click the button below to verify your email address:</p>
                        <form method='get' action='" + link + @"'>
                            <button type='submit' class='button'>Verify Email</button>
                        </form>
                        <p> You can also use the following link: " + link + @"
                    </div>
                        <div class='footer'>
                        <p>This email was sent by Royal Motors, located at Clemenceau St. Hamra, Beirut.</p>
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

        public static string CarSoldEmail(string user_name, string car_name)
        {
            string html = @"
<html>
    <head>
    <title>Royal Motors - Car Sold</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            font-size: 16px;
            color: #444444;
        }
        .header {
            background-color: #1c2f36;
            color: #ffffff;
            padding: 20px;
        }
        .header h1 {
            margin: 0;
            font-size: 28px;
        }
        p {
        font-size: 16px;
        color: #444;
        margin-bottom: 18px;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            background-color: #1c2f36;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 10px;
        }
        .button:hover {
            background-color: #388e3c;
        }
        .footer {
            background-color: #1c2f36; 
            padding: 20px;
            text-align: center;
            font-size: 14px;
            color: #ffffff;
        }
        body p{
            margin-left: 3rem;
        }
        body li{
            margin-left: 3rem;
        }
        h2{
        background-color: #1c2f36;
         color: #ffffff;
          padding: 20px;
        }
    </style>
    </head>

    <body>
        <div class='header'>
            <h1>Royal Motors</h1>
        </div>

        <div class='content'>
            <h2>" + car_name + @" Car Has Been Sold</h2>
            <p>Hello " + user_name + @",</p>
            <p>We wanted to let you know that the <strong>" + car_name + @"</strong> you were interested is <strong>no longer available</strong>. We appreciate your interest in our company and hope to assist you with your future car buying needs.</p>
            <p><img src='cid:image1' style='border: 2px solid #1c2f36; padding: 0px; width: 200px'></p>
            <p>If you have any questions or would like to learn more about our current inventory, please visit our website or contact us directly.</p>
            <a href=""https://royalmotors.pages.dev/"" class=""button"">Visit Our Website</a>
            <p>Thank you for choosing Royal Motors!</p>
        </div>
        <div class='footer'>
            <p style='color: #ffffff'>This email was sent by Royal Motors, located at Clemenceau St. Hamra, Beirut.</p>
        </div>
    </body>
        </html>";
            return html;
        }

        public static string TestdriveReminderEmail(string name, string date, string time, string car_name)
        {
            string html = @"
        <html>
    <head>
    <title>Royal Motors - Test Drive Reminder</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            font-size: 16px;
            color: #444444;
        }
        .header {
            background-color: #1c2f36;
            color: #ffffff;
            padding: 20px;
        }
        .header h1 {
            margin: 0;
            font-size: 28px;
        }
        p {
        font-size: 16px;
        color: #444;
        margin-bottom: 18px;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            background-color: #1c2f36;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 10px;
            margin-left: 3rem;
        }
        .button:hover {
            background-color: #388e3c;
        }
        .footer {
            background-color: #1c2f36; 
            padding: 20px;
            text-align: center;
            font-size: 14px;
            color: #ffffff;
        }
        body p{
            margin-left: 3rem;
        }
        body li{
            margin-left: 3rem;
        }
        h2{
            background-color: #1c2f36;
             color: #ffffff;
              padding: 20px;
        }

        h3{
            margin-left: 3rem;
        }

    </style>
    </head>

    <body>
        <div class='header'>
            <h1>Royal Motors</h1>
        </div>

        <div class='content'>
            <h2>Test Drive Reminder</h2>
            <p>Hello " + name +@",</p>
            <p>We wanted to remind you about your upcoming test drive appointment for the Car "+ car_name + @". Your appointment is scheduled for "+ date + @" at "+ time + @".</p>
            <p>Please remember to bring your driver's license and any other required documents with you to the dealership</p> <br>
            <p>If you need to reschedule or cancel your appointment, please contact us as soon as possible. We look forward to seeing you soon!</p>
            <a href=""https://royalmotors.pages.dev/"" class=""button"">Visit Our Website</a>
        </ div >
            < h3 > Appointment Details:</ h3 >
            < ul >
                < li > Date: " + date + @" </ li >
                < li > Time: " + time + @" </ li >
            </ ul >
            < p > Thank you for choosing Royal Motors! </ p >
        </ div >

        < div class='footer'>
            <p style = 'color: #ffffff' > This email was sent by Royal Motors, located at Clemenceau St. Hamra, Beirut.</p>
        </div>
    </body>
 </html>";
            return html;
        }

        public static string TestdriveCreatedEmail(string name, string date, string time, string car_name, string url)
        {
            string html = @"
<html>
    <head>
    <title>Royal Motors - Test Drive Appointment Reserved</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            font-size: 16px;
            color: #444444;
        }
        .header {
            background-color: #1c2f36;
            color: #ffffff;
            padding: 20px;
        }
        .header h1 {
            margin: 0;
            font-size: 28px;
        }
        p {
        font-size: 16px;
        color: #444;
        margin-bottom: 18px;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            background-color: #1c2f36;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 10px;
        }
        .button:hover {
            background-color: #388e3c;
        }
        .footer {
            background-color: #1c2f36; 
            padding: 20px;
            text-align: center;
            font-size: 14px;
            color: #ffffff;
        }
        body p{
            margin-left: 3rem;
        }
        body li{
            margin-left: 3rem;
        }
        h2{
            background-color: #1c2f36;
             color: #ffffff;
              padding: 20px;
        }

        h3{
            margin-left: 3rem;
        }

    </style>
    </head>

    <body>
        <div class='header'>
            <h1>Royal Motors</h1>
        </div>

        <div class='content'>
            <h2>Test Drive Reminder</h2>
            <p>Hello " + name + @",</p>
            <p>Your appointment for the <strong>" + car_name + @"</strong> has been succesfully created. Your appointment is scheduled for <strong>" + date + @"</strong> at <strong>" + time + @"</strong>.</p>
            <p><img src='" + url + @"' style='border: 2px solid #1c2f36; padding: 0px; width: 200px'></p>
            <p> Please remember to bring your <strong>driver's license</strong> and any other required documents with you to the dealership.</p>
            <p> If you need to reschedule or cancel your appointment, please contact us as soon as possible. We look forward to seeing you soon!</p>
            <a href = ""https://royalmotors.pages.dev/"" class=""button"">Visit Our Website</a>
            <p></p>
            <h3> Appointment Details:</h3>
            <ul>
                <li> Date: " + date + @" </li>
                <li> Time: " + time + @" </li>
            </ul>
            <p> Thank you for choosing Royal Motors! </p>
        </div>

        <div class='footer'>
            <p style = 'color: #ffffff' > This email was sent by Royal Motors, located at Clemenceau St.Hamra, Beirut.</p>
        </div>
    </body>
 </html>";
            return html;
        }

        public static string resetPasswordEmail(string code)
        {
            string emailBody = @"
            <html>
                <head>
                <title>Royal Motors - Reset Password</title>
                <style>
                    body {
                        margin: 0;
                        padding: 0;
                        font-family: Arial, sans-serif;
                        font-size: 16px;
                        color: #444444;
                        }
                    .header {
                        background-color: #1c2f36;
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
                        background-color: #1c2f36;
                        color: #ffffff;
                        text-decoration: none;
                        border-radius: 5px;
                        margin-top: 20px;
                        margin-left: 3rem;
                        }
                    .button:hover {
                        background-color: #95c197;
                    }
                    .footer {
                        background-color: #1c2f36;
                        padding: 20px;
                        text-align: center;
                        font-size: 14px;
                        color: white;
                        }


                        .content p{
                            margin-left: 3rem;
                        }
                </style>

                </head>

                <body>
                    <div class='header'>
                        <h1>Royal Motors</h1>
                    </div>
                    <div class='content'>
                        <h2 style='background-color: #1c2f36; color: #ffffff; padding: 20px;'>Verify your email address</h2>
                        <p>Use the code below to reset your password</p>
                        <p>" + code + @"</p>
                    </div>
                        <div class='footer'>
                        <p>This email was sent by Royal Motors, located at Clemenceau St. Hamra, Beirut.</p>
                    </div>
                </body>
            </html>";
            return emailBody;
        }
    }
}