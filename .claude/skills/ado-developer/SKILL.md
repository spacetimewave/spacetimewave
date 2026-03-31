---
name: ado-developer
description: Azure DevOps skill to read User Stories and Work Items within Project sprints and start implementing them and making PRs. 
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

3. Get iterations and sprints for the project.

```
! Bash(az boards iteration project list --depth 3 2>&1 | head -60)
```

4. Get work items (US, Bugs, and tasks) for a specific sprint and iteration path.

```
! Bash(az boards query --wiql "SELECT [System.Id], [System.WorkItemType], [System.Title],
      [System.State], [System.AssignedTo], [Microsoft.VSTS.Scheduling.StoryPoints] FROM WorkItems    
      WHERE [System.IterationPath] = 'Project Name\Iteration Path' ORDER 
      BY [System.WorkItemType], [System.Id]" 2>&1)
```

5. Get details of a specific work item.

```
! Bash(az boards work-item show --id 0000000 --expand all 2>&1) 

   US #0000000 — Title

   State: New | Priority: 2 | Created by: Javier Hernandez Sanchez

   Description:
   ▎ Description of the user story
```

To get more details about the work item, such as comments or acceptance criteria, you can use the following command:

```
! Bash(az devops invoke --area wit --resource comments --route-parameters project="Project Name" workItemId=0000000 --api-version 7.1 2>&1)
```

6. Create a new git branch and git worktree.

Please use the following naming convention: 
- branches: `/feature/#0000000-*` or `/fix/#0000000-*`
- worktrees: `./feature-#0000000-*` or `./fix-#0000000-*`

7. cd into the new worktree

8. Plan and implement the user story in the new worktree.

9. After implementing the user story create a new commit, push the new branch to the remote repository (origin) and create a pull request to merge your changes into the dev branch.

```
Bash(cd "C:/Projects/Project_Name/.claude/worktrees/#0000000-us" && az repos pr create --repository
      0503-ocu-currencies" && az repos pr create --repository
      Repository_Name --source-branch
      "fix/#0000000-us" --target-branch dev
      --title "fix: commit message" --description "$(cat <<'EOF'
       PR Description
      EOF
      )" 2>&1)
```

10. Link the work item to the pull request.

```
! Bash(az repos pr work-item add --id 371144 --work-items 0000000 2>&1)
```