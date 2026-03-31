---
name: ado-reviewer
description: Azure DevOps skill to review pull requests and provide feedback. 
---

0. [OPTIONAL] [SKIP] Install Azure CLI, login into to your Azure account and select your Azure tenant and subscription.

```
$ az upgrade
$ az account clear
$ az config set core.enable_broker_on_windows=false
$ az login     

   A web browser has been opened at https://login.microsoftonline.com/organizations/oauth2/v2.0/authorize. Please continue the login in the web browser. If no web browser is available or if the web browser fails to open, use device code flow with `az login --use-device-code`.

   Retrieving tenants and subscriptions for the selection...
   The following tenants don't contain accessible subscriptions. Use `az login --allow-no-subscriptions` to have tenant level access.

   [Tenant and subscription selection]

   No     Subscription name    Subscription ID                       Tenant
   -----  -------------------  ------------------------------------  ----------------
   [1]    subscription    6ef60186-7351...a805e  Tenant-1
   [2]    subscription    5e60a9cf-2aca...d08cf  Tenant-1 *
   [3]    subscription    00000000-0000...00000  Tenant-2
   [4]    subscription    00000000-0000...00000  Tenant-2

   The default is marked with an *; the default tenant is 'Tenant-1' and subscription is 'subscription' (5e60a9cf-2aca-400b-b424-dfa5888d08cf).

   Select a subscription and tenant (Type a number or Enter for no changes): 2
```

1. [OPTIONAL] [SKIP] Install Azure DevOps extension for Azure CLI and login into your Azure DevOps account.

```
$ $ENV:PYTHONPATH = "C:\\Program Files\\Microsoft SDKs\\Azure\\CLI2"
$ az extension add --name azure-devops                              
```

2. Set the default Azure DevOps organization and project.

```
$ az devops configure --defaults organization=https://dev.azure.com/org project="Project Name"  
```

```
! Bash(export PYTHONPATH="C:\\Program Files\\Microsoft SDKs\\Azure\\CLI2"
```

1. List active pull requests for the project if PR ID is not specified.

```
! Bash(az repos pr list --repository <project> --output table 2>&1 | head -50)
```

2. Get details of a specific pull request, including linked work items.

```
! Bash(az repos pr show --id 371144 --output json 2>&1)
```
       
3. Load the PR branch in a git worktree and review the code changes. Provide feedback (comments).

```
! Bash(git fetch origin fix/#0000000-us 2>&1)
```

4. Add a reviewer to the pull request.

```
az repos pr reviewer add --id 371144 --reviewers "example@example.com" 2>&1
```

5. Add comments to the pull request.

Get all threads for the pull request:

```
az devops invoke --area git --resource pullRequestThreads --org https://dev.azure.com/<your_org> --route-parameters project=<your_project> repositoryId=<repo_name> pullRequestId=<pr_id> --http-method GET --api-version 5.1-preview -o json 
```

Get comments for a specific thread:

```
az devops invoke --area git --resource pullRequestThreadComments --org https://dev.azure.com/<your_org> --route-parameters project=<your_project> repositoryId=<repo_name> pullRequestId=<pr_id> threadId=<id_from_previous_cmd> --http-method GET --api-version 5.1-preview -o json
```

Create a pr-comment.json file with the PR comment content:

```
{
  "comments": [
    {
      "parentCommentId": 0,
      "content": "...comment content here...",
      "commentType": 1
    }
  ],
  "status": 1
}

```

Publish the PR comment to a specific thread:

```
az devops invoke \
        --area git \
        --resource pullRequestThreads \
        --org https://dev.azure.com/org \
        --route-parameters project="Project Name" \
        repositoryId="Repository Name" pullRequestId=371144 \
        --http-method POST \
        --api-version 5.1-preview \
        --in-file "C:/Temp/pr-comment.json" \
        -o json 2>&1)
```