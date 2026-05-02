# 🖋️ PDF Digital Signature System

A professional web application built with **.NET 10 (ASP.NET Core MVC)** that allows users to upload PDF documents, create hand-drawn digital signatures, and apply them to specific pages with precision.

---

## 🚀 Technical Stack

- **Framework:** .NET 10 (ASP.NET Core MVC)
- **Architecture:** Clean layered structure (Controllers, Services, Data)
- **Design Patterns:** Repository Pattern, Dependency Injection, Separation of Concerns
- **Database:** SQL Server (Entity Framework Core - Code First)
- **Authentication:** ASP.NET Core Identity
- **PDF Engine:** iText7 + Bouncy Castle Adapter

---

## 📦 Key NuGet Packages


dotnet add package itext7
dotnet add package itext7.bouncy-castle-adapter
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.sqlserver
dotnet add package Microsoft.AspNetCore.Tools
dotnet add package Microsoft.AspNetCore.design
dotnet add package Microsoft.AspNetCore.Identity.UI

🛠️ Key Features
----------------

### 🔐 Identity Management

*   Secure Register / Login system
    
*   Each user accesses only their own documents and signatures
    

### 📤 PDF Upload & Management

*   Upload PDF documents
    
*   Stored in /wwwroot/uploads
    
*   View documents directly in browser
    

### ✍️ Hand-Drawn Signatures

*   HTML5 Canvas for drawing signatures
    
*   Saved as transparent PNG images
    
*   Stored in /wwwroot/signatures
    

### 📄 PDF Signing

*   Sign all pages OR specific pages (e.g., 1,3,5)
    
*   Dynamic placement:
    
    *   Top Right
        
    *   Bottom Right
        
*   Uses **iText7** for manipulation
    

### 📚 Signed Documents History

*   Track all signed documents
    
*   Download previously signed files
    
*   Stored in /wwwroot/signed
    

### 🧱 Project Structure
--------------------

   /Controllers    AuthController    DocumentsController    SignatureController/Models    Document    Signature    SignedDocument/Data    AppDbContext/Services    IPdfSignerService    PdfSignerService/wwwroot    /uploads    /signatures    /signed   `

### 🗄️ Database Schema
-------------------

### Documents

*   Id
    
*   UserId
    
*   FileName
    
*   FilePath
    
*   UploadedAt
    

### Signatures

*   Id
    
*   UserId
    
*   SignatureImagePath
    
*   CreatedAt
    
*   IsDefault
    

### SignedDocuments

*   Id
    
*   DocumentId
    
*   UserId
    
*   SignedFilePath
    
*   SignedAt
    
*   SignatureId
    

### 📑 Core Logic & Methods
-----------------------

### 1\. DocumentsController

*   **Index()**
    
    *   Lists all user documents
        
*   **Upload()**
    
    *   Handles PDF upload
        
    *   Saves file to /uploads
        
*   **SignedHistory()**
    
    *   Returns signed documents history
        

### 2\. SignatureController

*   **Create() (POST)**
    
    *   Converts Base64 canvas image to PNG
        
    *   Saves signature to /signatures
        
*   **Sign(int docId) (GET)**
    
    *   Loads document for signing
        
*   **ProcessSigning() (POST)**
    
    *   Handles:
        
        *   Selected signature
            
        *   Page selection
            
        *   Position
            
        *   Calls PDF service
            

### 3\. PdfSignerService

*   **SignDocumentAsync()**
    
    *   Loads original PDF
        
    *   Injects signature image
        
    *   Writes a new signed file
        
    *   Ensures original file is untouched
        

### ⚙️ How It Works
---------------

1.  User uploads a PDF
    
2.  User creates or selects a signature
    
3.  User selects:
    
    *   Pages (all or specific)
        
    *   Position (top/bottom)
        
4.  System processes PDF using iText7
    
5.  Signed PDF is saved and available for download
    

### 🏃 How to Run the Project
-------------------------

### ✅ Prerequisites

*   .NET 10 SDK
    
*   SQL Server
    

### 🗃️ Database Setup

   Update-Database   `

### 📁 Required Folders

Make sure these exist inside **wwwroot**:

   /uploads/signatures/signed   `



### ▶️ Run Project

   dotnet run   `

### ⚠️ Important Notes
------------------

*   Original PDFs are never modified
    
*   All files stored using unique GUID names
    
*   Always validate user ownership before operations
    
*   Signature images are stored as PNG for transparency
    

### 💡 Future Enhancements
----------------------

*   Choose signature positioning
        
*   Multiple signatures per document
            
