# ---------- Stage 1: Builder ----------
FROM node:22.16.0-alpine AS builder

WORKDIR /app

# Install pnpm
RUN npm install -g pnpm

# 1. Copy only files needed for installation
COPY package.json pnpm-lock.yaml ./

# 2. Install dependencies (this creates the symlinked node_modules)
RUN pnpm install --frozen-lockfile

# 3. Copy the rest of the source code (exclude local node_modules via .dockerignore)
COPY . .

# 4. CRITICAL: If you have a local node_modules, it might have overwritten the 
# container's versions. We ensure the build sees the installed Vite.
RUN pnpm run build:local

# ---------- Stage 2: Production ----------
FROM node:22.16.0-alpine

WORKDIR /app

RUN npm install -g serve

# Copy built files from builder
COPY --from=builder /app/dist ./dist

EXPOSE 3000

CMD ["serve", "-s", "dist", "-l", "3000"]