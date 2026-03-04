# Google reCAPTCHA Implementation - Complete Guide

## ? What Has Been Implemented

### **Models Created**
1. **FeedBackViewModel.cs** - View model for form binding with validation attributes
2. **FeedBack.cs** - Entity model for database persistence
3. **RecaptchaResponse.cs** - Model for Google reCAPTCHA API response

### **Services Created**
1. **RecaptchaService.cs** - Service for server-side reCAPTCHA validation
   - Interface: `IRecaptchaService`
   - Implementation: `RecaptchaService`
   - Registered as scoped service in Program.cs

### **Pages Updated**

#### **1. index.cshtml (Home Page)**
- ? Added reCAPTCHA widget to contact form
- ? Implemented model binding with `FeedBackViewModel`
- ? Added validation error messages
- ? Added status message display (success/error)
- ? Included reCAPTCHA script

#### **2. index.cshtml.cs**
- ? Added dependency injection for `DbContext` and `IRecaptchaService`
- ? Implemented `OnPostAsync` method with:
  - Server-side reCAPTCHA validation
  - Model validation
  - Database save operation
  - Error handling
  - Success/Error status messages

#### **3. contact-us.cshtml**
- ? Updated form with proper model binding
- ? Added reCAPTCHA widget
- ? Added validation error messages
- ? Added status message display
- ? Included reCAPTCHA script and validation scripts

#### **4. contact-us.cshtml.cs**
- ? Same implementation as index.cshtml.cs
- ? Server-side reCAPTCHA validation
- ? Database save operation

### **Configuration Files Updated**

#### **Program.cs**
```csharp
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRecaptchaService, RecaptchaService>();
```

#### **appsettings.json**
```json
"ReCaptcha": {
  "SiteKey": "YOUR_RECAPTCHA_SITE_KEY_HERE",
  "SecretKey": "YOUR_RECAPTCHA_SECRET_KEY_HERE"
}
```

## ?? Configuration Steps

### **Step 1: Get Your reCAPTCHA Keys**
1. Go to: https://www.google.com/recaptcha/admin/create
2. Register your site:
   - **Label**: Bethel Heritage Academy
   - **reCAPTCHA type**: v2 ? "I'm not a robot" Checkbox
   - **Domains**:
     - Development: `localhost`
     - Production: `yourdomain.com`
3. Copy both keys (Site Key and Secret Key)

### **Step 2: Update Configuration**

#### **A. Update appsettings.json**
Replace the placeholders in `appsettings.json`:
```json
"ReCaptcha": {
  "SiteKey": "6LcYOUR-ACTUAL-SITE-KEY",
  "SecretKey": "6LcYOUR-ACTUAL-SECRET-KEY"
}
```

#### **B. Update index.cshtml**
Find line ~421 and replace:
```html
<div class="g-recaptcha" data-sitekey="YOUR_RECAPTCHA_SITE_KEY_HERE"></div>
```
With:
```html
<div class="g-recaptcha" data-sitekey="6LcYOUR-ACTUAL-SITE-KEY"></div>
```

#### **C. Update contact-us.cshtml**
Find the reCAPTCHA div and replace with your actual site key.

### **Step 3: Setup Database Context**

**IMPORTANT**: The PageModels reference `DbContext` which needs to be injected properly.

Update both `index.cshtml.cs` and `contact-us.cshtml.cs` to use your actual DbContext:

```csharp
private readonly YourActualDbContext _context; // Replace with your DbContext name

public indexModel(YourActualDbContext context, IRecaptchaService recaptchaService)
{
    _context = context;
    _recaptchaService = recaptchaService;
}
```

Example if you have ApplicationDbContext:
```csharp
private readonly ApplicationDbContext _context;

public indexModel(ApplicationDbContext context, IRecaptchaService recaptchaService)
{
    _context = context;
    _recaptchaService = recaptchaService;
}
```

### **Step 4: Add FeedBack DbSet to Your DbContext**

Add this to your DbContext class:
```csharp
public DbSet<FeedBack> FeedBacks { get; set; }
```

### **Step 5: Create Database Migration**

Run these commands in Package Manager Console or Terminal:

```bash
# Add migration
dotnet ef migrations add AddFeedBackTable

# Update database
dotnet ef database update
```

## ?? Testing the Implementation

### **1. Run the Application**
```bash
dotnet run
```

### **2. Test on Index Page**
1. Navigate to the home page (`/`)
2. Scroll to the contact section
3. Try submitting without filling form ? Validation errors should appear
4. Fill the form but don't complete reCAPTCHA ? Should show reCAPTCHA error
5. Complete all fields and reCAPTCHA ? Should save to database and show success message

### **3. Test on Contact-Us Page**
1. Navigate to `/contact-us`
2. Repeat the same tests as above
3. Verify server-side validation works
4. Check database for saved feedback

## ?? Security Features Implemented

? **Client-Side Validation** (ASP.NET Core Validation)
? **Server-Side Validation** (Model Validation)
? **reCAPTCHA v2 Verification** (Client & Server)
? **Model Binding** (Prevents over-posting)
? **Error Handling** (Try-catch blocks)
? **Status Messages** (TempData for user feedback)

## ?? File Structure

```
Bethel-Heritage-Academy-WebSite/
??? Models/
?   ??? FeedBack.cs
?   ??? FeedBackViewModel.cs
?   ??? RecaptchaResponse.cs
??? Services/
?   ??? RecaptchaService.cs
??? Pages/
?   ??? index.cshtml
?   ??? index.cshtml.cs
?   ??? contact-us.cshtml
?   ??? contact-us.cshtml.cs
??? appsettings.json
??? Program.cs
```

## ?? Important Notes

### **1. DbContext Reference**
The current implementation uses generic `DbContext`. You MUST replace this with your actual DbContext class name throughout the code.

### **2. reCAPTCHA Keys**
- **Site Key**: Goes in HTML (visible to users)
- **Secret Key**: Stays on server (in appsettings.json)
- **Never commit secret keys to version control**

### **3. Production Deployment**
For production, use:
- Azure Key Vault for secret storage
- Environment variables
- User Secrets (for development)

```bash
# Store secret key securely
dotnet user-secrets set "ReCaptcha:SecretKey" "your-secret-key"
```

## ?? Troubleshooting

### **Build Errors**
If you get DbContext-related errors:
1. Replace all `DbContext` references with your actual DbContext class
2. Ensure DbContext is registered in Program.cs
3. Ensure Entity Framework packages are installed

### **reCAPTCHA Not Showing**
1. Check browser console for errors
2. Verify site key is correct
3. Check if domain is registered
4. Disable ad blockers

### **Server Validation Fails**
1. Verify secret key is correct in appsettings.json
2. Check network connectivity to Google servers
3. Ensure HttpClient is registered in Program.cs

### **Database Save Fails**
1. Verify connection string
2. Ensure migrations are applied
3. Check DbContext is properly injected
4. Verify FeedBack DbSet exists

## ?? What Happens When Form is Submitted

1. **Client-Side**: ASP.NET Core client validation runs
2. **User**: Completes reCAPTCHA
3. **Server-Side**:
   - Form posted to `OnPostAsync`
   - reCAPTCHA token validated with Google API
   - Model validation checks all required fields
   - Data mapped from ViewModel to Entity
   - Saved to database via DbContext
   - Success/Error message set in TempData
   - Redirects to same page (PRG pattern)
4. **User**: Sees success/error message

## ?? Next Steps

1. **Replace DbContext reference** with your actual context
2. **Add your reCAPTCHA keys** to config files and views
3. **Run database migrations** to create FeedBack table
4. **Test both forms** thoroughly
5. **Add email notifications** (optional enhancement)
6. **Create admin panel** to view feedback (optional)

## ?? Email Notification (Optional Enhancement)

To send email notifications when feedback is received, add this service:

```csharp
// IEmailService.cs
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

// In OnPostAsync, after SaveChangesAsync:
await _emailService.SendEmailAsync(
    "bethelheritateedu@gmail.com",
    $"New Feedback: {feedback.Subject}",
    $"From: {feedback.Name} ({feedback.Email})\n\nMessage: {feedback.Message}"
);
```

## ? Checklist Before Deployment

- [ ] Replace all DbContext references with actual context
- [ ] Add reCAPTCHA Site Key to both views
- [ ] Add reCAPTCHA Secret Key to appsettings
- [ ] Run database migrations
- [ ] Test form submission on both pages
- [ ] Verify reCAPTCHA validation works
- [ ] Test error handling
- [ ] Configure production reCAPTCHA keys
- [ ] Use secure storage for secret keys
- [ ] Test on production domain

---

**Need Help?** Check the RECAPTCHA_SETUP.md and IMPLEMENTATION_SUMMARY.md files for additional information.
