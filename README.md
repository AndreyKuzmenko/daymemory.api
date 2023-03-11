# Day Memory API
Day Memory is a note taking app that preserves your memories.
### Mobile and Desktop Clients
There is a separate project built in Flutter https://github.com/AndreyKuzmenko/daymemory.mobile

## TODO
Add instructions on how to set up the project

Start by configuring the appsettings.
1. Azure storage
- StorageConnectionString
- BlobStorageRootUrl - Azure fileUrl, e.g. https://[name].blob.core.windows.net
- RetorePasswordUrlTemplate - web page to restore a password. e.g. http://localhost:3000/account/restore-password?token={0}&email={1}
2. Secret - JWT token key, e.g. 6B42FAF58C77D650293FC1BF2183DE0D2
3. Connection String - SQL Server Connection String
4. EmailSender - Mail Settings
