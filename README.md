# foERP

foERP 是一个参考 **Dynamics 365 Finance & Operations** 业务域设计的企业级 ERP 基座，技术栈如下：

- 后端：C# .NET 8
- 架构：DDD + Clean Architecture
- 数据库：SQL Server 2022
- 前端：TypeScript + React

> 说明：完整“功能 100% 对齐 Dynamics 365 F&O”是一个超大型多年项目，本仓库提供的是可落地的企业级起步骨架，覆盖核心业务域边界、统一建模规范、集成方式与实施路线。

## 目录结构

```text
backend/
  src/
    foERP.Domain/
    foERP.Application/
    foERP.Infrastructure/
    foERP.API/
frontend/
  src/
docs/
```

## 如何下载

### 方式 1：使用 Git（推荐）

```bash
git clone <你的仓库地址>
cd ems
```

### 方式 2：下载 ZIP

1. 打开仓库页面
2. 点击 **Code** → **Download ZIP**
3. 解压后进入项目目录 `ems`

## 环境要求

- Node.js 20+
- npm 10+
- .NET 8 SDK
- Docker（用于本地 SQL Server 2022）

## 业务域（与 F&O 对齐的能力分区）

- 财务管理（总账、应收、应付、固定资产、现金银行）
- 供应链管理（库存、仓储、主计划、生产）
- 采购与寻源
- 销售与分销
- 人力资源与薪资
- 项目管理与核算

## 快速启动（示意）

### 1) 启动 SQL Server 2022

```bash
docker compose up -d
```

### 2) 启动前端

```bash
cd frontend
npm install
npm run dev
```

### 3) 启动后端（本地有 .NET 8 时）

```bash
cd backend/src/foERP.API
# 可先检查 backend/src/foERP.API/appsettings.json 连接串
dotnet restore
dotnet run
```

## 下一步

请先阅读：

- `docs/roadmap.md`
- `docs/domain-map.md`
- `backend/src/foERP.API/appsettings.json`
