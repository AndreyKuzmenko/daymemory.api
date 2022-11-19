# Day Memory Api Project

Start by configuring the appsettings. For that you need to have an azure blob storage.
1. Azure storage
- StorageConnectionString
- BlobStorageRootUrl - Azure fileUrl, e.g. https://[name].blob.core.windows.net
- RetorePasswordUrlTemplate - web page to restore a password. e.g. http://localhost:3000/account/restore-password?token={0}&email={1}
2. Secret - JWT token key, e.g. 6B42FAF58C77D650293FC1BF2183DE0D2
3. Connection String - SQL Server Connection String
4. EmailSender - Mail Settings
