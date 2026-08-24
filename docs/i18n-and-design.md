# 🌐 Internationalization (i18n) & Design System - GlassHub

![Design System Card](https://glasshub-quasar.vercel.app/api/card?title=i18n+%26+Glassmorphism+Design&description=Dynamic+localization+system+and+GlassHub+cosmic+design+tokens.&tag=DESIGN+SYSTEM&icon=sparkles&width=650&theme=glass-dark)

This document outlines the internationalization architecture and visual design system guidelines of **GlassHubEventHorizon**, aligned with the **GlassHub Ecosystem**.

---

## 🌍 Internationalization Architecture (i18n)

GlassHubEventHorizon features a strongly-typed, dynamic i18n localization system supporting:
- **`en-US`**: English (default international standard).
- **`pt-BR`**: Brazilian Portuguese.

### Translation Interface (`LocalizationService`)

```csharp
public interface ILocalizationService
{
    string CurrentLanguage { get; }
    void SetCulture(string cultureCode);
    string GetString(string key);
    string Format(string key, params object[] args);
}
```

### Key Capabilities:
1. **Automatic Operating System Detection:** Detects current system OS language upon application startup.
2. **Dynamic Runtime Override (`--lang`):** Allows changing language culture on the fly via `evh` CLI arguments or GUI header buttons.
3. **Resilient Fallback:** If a string key is missing in the selected locale, the system gracefully falls back to `en-US` without crashing.

---

## 🎨 GlassHub Ecosystem Design Guidelines

The visual interface of GlassHubEventHorizon adopts the signature **Cosmic Glassmorphism** design language of GlassHub:

### 🌌 1. Color Palette & Design Tokens
- **Primary Background:** `#0D0F1A` (Deep cosmic dark space navy).
- **Glassmorphic Cards:** `#1E2235` with translucency and specular `1px` border reflections in `#33FFFFFF`.
- **Neon Cyan Accent (Primary Actions / Compression):** `#00F2FE`.
- **Neon Magenta Accent (Extraction / Secondary Actions):** `#4FACFE`.
- **Primary Text:** `#F0F4F8` (High contrast WCAG AA compliant).
- **Secondary Text:** `#A0AEC0` (Soft silver gray).

---

### ✨ 2. Glassmorphic Aesthetics & Micro-interactions
- **Translucency & Layering:** Layered UI elements creating depth and cosmic dimensional perspective.
- **Hover Micro-interactions:** Buttons and cards respond to mouse cursor hover with subtle glowing neon borders.
- **Progress Telemetry:** Smooth 60 FPS terminal and GUI progress telemetry bar rendering.

---

## ♿ 3. Accessibility & Usability
- **Clear Semantics:** All buttons, fields, and options have explicit descriptive labels.
- **Instantaneous Feedback:** Every action produces immediate visual telemetry (`[SUCCESS]`, `[FAIL]`, progress indicators).
- **Multilingual Support:** Complete localization for error tracebacks and technical instruction.
