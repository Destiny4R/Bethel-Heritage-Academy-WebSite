# Google reCAPTCHA Setup Instructions

## Overview
Google reCAPTCHA v2 has been implemented on the Contact Us page with comprehensive client-side validation.

## Getting Your reCAPTCHA Keys

1. **Visit Google reCAPTCHA Admin Console**
   - Go to: https://www.google.com/recaptcha/admin/create

2. **Register Your Site**
   - **Label**: Give your site a label (e.g., "Bethel Heritage Academy Contact Form")
   - **reCAPTCHA type**: Select "reCAPTCHA v2" > "I'm not a robot Checkbox"
   - **Domains**: Add your domain(s)
     - For development: `localhost`
     - For production: `yourdomain.com` (without http:// or https://)
   - Accept the reCAPTCHA Terms of Service
   - Click "Submit"

3. **Copy Your Keys**
   - You will receive two keys:
     - **Site Key** (Public key - used in the frontend)
     - **Secret Key** (Private key - used for server-side validation)

## Configuration Steps

### Step 1: Update the Site Key
Open the file: `Bethel-Heritage-Academy-WebSite\Pages\contact-us.cshtml`

Find this line (around line 57):
```html
<div class="g-recaptcha" data-sitekey="YOUR_RECAPTCHA_SITE_KEY_HERE"></div>
```

Replace `YOUR_RECAPTCHA_SITE_KEY_HERE` with your actual Site Key:
```html
<div class="g-recaptcha" data-sitekey="6LcExample_Your_Actual_Site_Key_Here"></div>
```

### Step 2: Server-Side Validation (Recommended)
For production use, you should also validate the reCAPTCHA response on the server.

Update `Bethel-Heritage-Academy-WebSite\Pages\contact-us.cshtml.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bethel_Heritage_Academy_WebSite.Pages
{
    public class contact_usModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public contact_usModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ContactFormModel Contact { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validate reCAPTCHA
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                ModelState.AddModelError("", "Please complete the reCAPTCHA verification.");
                return Page();
            }

            var isRecaptchaValid = await ValidateRecaptchaAsync(recaptchaResponse);
            if (!isRecaptchaValid)
            {
                ModelState.AddModelError("", "reCAPTCHA validation failed. Please try again.");
                return Page();
            }

            // Process the form (send email, save to database, etc.)
            // ... your form processing logic here ...

            TempData["Success"] = "Thank you for contacting us! We'll get back to you soon.";
            return RedirectToPage();
        }

        private async Task<bool> ValidateRecaptchaAsync(string recaptchaResponse)
        {
            var secretKey = _configuration["ReCaptcha:SecretKey"];
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}",
                null);

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RecaptchaResponse>(jsonString);

            return result?.success ?? false;
        }
    }

    public class ContactFormModel
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class RecaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
```

### Step 3: Store Secret Key Securely
Add your reCAPTCHA secret key to `appsettings.json`:

```json
{
  "ReCaptcha": {
    "SecretKey": "YOUR_RECAPTCHA_SECRET_KEY_HERE"
  }
}
```

**IMPORTANT**: Never commit your secret key to version control. Use User Secrets for development:
```bash
dotnet user-secrets set "ReCaptcha:SecretKey" "your-secret-key-here"
```

For production, use Azure Key Vault or environment variables.

### Step 4: Register HttpClientFactory
In `Program.cs`, add:
```csharp
builder.Services.AddHttpClient();
```

## Features Implemented

### Client-Side Validation
? Full name validation (required, minimum 3 characters)
? Phone number validation (required, valid format, minimum 10 characters)
? Email validation (required, valid email format)
? Subject validation (required)
? Message validation (required, minimum 10 characters)
? reCAPTCHA validation (required)
? Real-time validation on blur
? Error messages for each field
? Submit button disabled during submission
? Form reset after successful submission

### Security Features
? reCAPTCHA v2 bot protection
? Input sanitization
? Required field enforcement
? Format validation

## Testing

### Local Testing
1. Update the site key in `contact-us.cshtml`
2. Run the application
3. Navigate to the Contact Us page
4. The reCAPTCHA widget should appear
5. Try submitting without filling the form - validation errors should appear
6. Fill the form and complete the reCAPTCHA
7. Submit the form

### Production Deployment
- Ensure your production domain is added to the reCAPTCHA admin console
- Update the site key if using different keys for development/production
- Test the reCAPTCHA on your production domain
- Monitor failed verification attempts in the reCAPTCHA admin console

## Troubleshooting

### reCAPTCHA not showing
- Check browser console for errors
- Verify the site key is correct
- Ensure the domain is registered in reCAPTCHA admin
- Check if ad blockers are interfering

### Validation always fails
- Verify the secret key is correct
- Check server-side validation code
- Ensure the domain matches what's registered
- Check network requests in browser DevTools

## Additional Resources
- [Google reCAPTCHA Documentation](https://developers.google.com/recaptcha/docs/display)
- [reCAPTCHA Admin Console](https://www.google.com/recaptcha/admin)
- [ASP.NET Core Form Validation](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation)
