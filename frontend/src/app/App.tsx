const modules = [
  { key: 'finance', name: '财务管理', capability: '总账 / 应收 / 应付 / 固定资产' },
  { key: 'supply-chain', name: '供应链管理', capability: '库存 / 仓储 / 主计划 / 生产' },
  { key: 'procurement', name: '采购与寻源', capability: '采购申请 / 采购订单 / 供应商协同' },
  { key: 'sales', name: '销售与分销', capability: '报价 / 销售订单 / 发运与开票' },
  { key: 'hcm', name: '人力资源', capability: '组织架构 / 岗位 / 员工生命周期' },
  { key: 'project', name: '项目管理', capability: '项目预算 / 工时 / 成本控制' }
];

const accountBooks = [
  { code: 'CN-BOOK', name: '中国账套', currency: 'CNY', status: '启用' },
  { code: 'US-BOOK', name: '美国账套', currency: 'USD', status: '启用' },
  { code: 'EU-BOOK', name: '欧洲账套', currency: 'EUR', status: '停用' }
];

export function App() {
  const activeModule = modules[0];

  return (
    <div className="shell">
      <aside className="sidebar">
        <div className="brand">foERP</div>
        <p className="subtitle">Finance & Operations</p>
        <nav className="nav">
          {modules.map((module, index) => (
            <button
              key={module.key}
              className={`nav-item ${index === 0 ? 'active' : ''}`}
              type="button"
            >
              <span className="dot" />
              <span>{module.name}</span>
            </button>
          ))}
        </nav>
      </aside>

      <main className="content">
        <header className="content-header">
          <h1>{activeModule.name}</h1>
          <p>{activeModule.capability}</p>
        </header>

        <section className="panel-grid">
          <article className="panel-card account-books">
            <div className="row-title">
              <h2>多账套管理</h2>
              <span className="badge">F&O 风格</span>
            </div>
            <table>
              <thead>
                <tr>
                  <th>账套编码</th>
                  <th>账套名称</th>
                  <th>本位币</th>
                  <th>状态</th>
                </tr>
              </thead>
              <tbody>
                {accountBooks.map((book) => (
                  <tr key={book.code}>
                    <td>{book.code}</td>
                    <td>{book.name}</td>
                    <td>{book.currency}</td>
                    <td>{book.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </article>

          {modules.slice(1).map((module) => (
            <article key={module.key} className="panel-card">
              <h2>{module.name}</h2>
              <p>{module.capability}</p>
            </article>
          ))}
        </section>
      </main>
    </div>
  );
}
