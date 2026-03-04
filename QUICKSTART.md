# ?? Quick Start - Google reCAPTCHA Setup

## ? 3-Minute Setup Guide

### **Step 1: Get Your Keys (2 minutes)**

1. Go to: **https://www.google.com/recaptcha/admin/create**
2. Fill in:
   - Label: `Bethel Heritage Academy`
   - Type: `reCAPTCHA v2` ? `"I'm not a robot" Checkbox`
   - Domains: `localhost` and `bethelheritageacademy.com`
3. Click **Submit**
4. **COPY BOTH KEYS** (Site Key and Secret Key)

---

### **Step 2: Update 3 Files (1 minute)**

#### **File 1: `appsettings.json`**
Replace `YOUR_RECAPTCHA_SITE_KEY_HERE` and `YOUR_RECAPTCHA_SECRET_KEY_HERE` with your actual keys.

#### **File 2: `Pages\index.cshtml`** (line ~421)
```html
<div class="g-recaptcha" data-sitekey="PASTE_YOUR_SITE_KEY_HERE"></div>
```

#### **File 3: `Pages\contact-us.cshtml`** (line ~46)
```html
<div class="g-recaptcha" data-sitekey="PASTE_YOUR_SITE_KEY_HERE"></div>
```

---

### **Step 3: Run Migration**
```bash
dotnet ef migrations add AddFeedBackTable
dotnet ef database update
```

---

### **Step 4: Test**
```bash
dotnet run
```

Visit `https://localhost:XXXX/` and test the contact form!

---

## ? That's It!

Both forms (index and contact-us) now have:
- ? reCAPTCHA protection
- ? Server-side validation
- ? Database integration

See **FINAL_SETUP_INSTRUCTIONS.md** for detailed information.

---

## ?? Find & Replace Summary

**Search for:** `YOUR_RECAPTCHA_SITE_KEY_HERE`
**Replace with:** Your actual Site Key (3 places total)

**Search for:** `YOUR_RECAPTCHA_SECRET_KEY_HERE`  
**Replace with:** Your actual Secret Key (1 place)

Done! ??
