# Claude Code 

# User Instructions

1. Install Claude Code.

2. Create .claude/settings.local.json and fill it with you API key.

    ```json
    {
        "$schema": "https://json.schemastore.org/claude-code-settings.json",
        "autoUpdatesChannel": "latest",
        "model": "eu.anthropic.claude-sonnet-4-6",
        "env": {
            "ANTHROPIC_BASE_URL": "...",
            "ANTHROPIC_AUTH_TOKEN": "...",
            "ANTHROPIC_MODEL": "eu.anthropic.claude-sonnet-4-6",
            "ANTHROPIC_DEFAULT_OPUS_MODEL": "eu.anthropic.claude-opus-4-6",
            "ANTHROPIC_DEFAULT_SONNET_MODEL": "eu.anthropic.claude-sonnet-4-6",
            "ANTHROPIC_DEFAULT_HAIKU_MODEL": "eu.anthropic.claude-haiku-4-5",
            "CLAUDE_CODE_SUBAGENT_MODEL": "eu.anthropic.claude-haiku-4-5"
        }
    }
    ```

3. Execute Claude Code (use repository root directory):

    ```console
    $env:NODE_TLS_REJECT_UNAUTHORIZED=0
    claude
    ```


