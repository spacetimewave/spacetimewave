---
name: claude-cost-explorer
description: Provide claude cost for the current conversation based on token consumption (context)
---

1. Get current token consumption for the conversation context, including system prompt, memory files, skills, messages, and free space. 

´´´
/context
  ⎿  Context Usage
     ⛀ ⛀ ⛀ ⛁ ⛁   eu.anthropic.claude-sonnet-4-6 · 84k/200k tokens
     (42%)
     ⛁ ⛁ ⛁ ⛶ ⛶
     ⛶ ⛶ ⛶ ⛶ ⛶   Estimated usage by category
     ⛶ ⛶ ⛶ ⛶ ⛶   ⛁ System prompt: 4.2k tokens (2.1%)
     ⛶ ⛝ ⛝ ⛝ ⛝   ⛁ Memory files: 1.6k tokens (0.8%)
                 ⛁ Skills: 401 tokens (0.2%)
                 ⛁ Messages: 42.9k tokens (21.4%)
                 ⛶ Free space: 118k (59.0%)
                 ⛝ Autocompact buffer: 33k tokens (16.5%)

     Memory files · /memory
     └ .claude\CLAUDE.md: 1.6k tokens

     Skills · /skills

     Project
     └ aws-azure-login: 26 tokens
     └ entity-framework: 26 tokens
´´´

2. Multiply the total token consuption by the token cost (Sonnet 4.5/4.6 at 3$ input and 15$ output per 1M tokens) to get the estimated cost for the conversation. 

```