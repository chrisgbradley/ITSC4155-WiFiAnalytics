# NinerFi Analytics

## Overview

## Use

### Requirements

### Recommendations

### Installation

### How to Use


---
## The Team

---
## Developer Notes

### Updating Entity Framework Scaffold
```powershell
dotnet ef dbcontext scaffold "Name=ConnectionStrings:NINERFI" Microsoft.EntityFrameworkCore.SqlServer --output-dir API/Data/Models --context-dir API/Data --context NINERFIContext -t "dbo.vwErrorTracking" -t "dbo.vwLogCount" -t "dbo.vwTrafficStats" -f --no-build --no-pluralize
```
