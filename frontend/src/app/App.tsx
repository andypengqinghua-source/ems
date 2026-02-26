import { useEffect, useMemo, useState } from 'react';

type ModuleItem = {
  key: string;
  name: string;
  capability: string;
};

type AccountBook = {
  code: string;
  name: string;
  currency: string;
  status: '启用' | '停用';
};

const modules: ModuleItem[] = [
  { key: 'finance', name: '财务管理', capability: '总账 / 应收 / 应付 / 固定资产' },
  { key: 'supply-chain', name: '供应链管理', capability: '库存 / 仓储 / 主计划 / 生产' },
  { key: 'procurement', name: '采购与寻源', capability: '采购申请 / 采购订单 / 供应商协同' },
  { key: 'sales', name: '销售与分销', capability: '报价 / 销售订单 / 发运与开票' },
  { key: 'hcm', name: '人力资源', capability: '组织架构 / 岗位 / 员工生命周期' },
  { key: 'project', name: '项目管理', capability: '项目预算 / 工时 / 成本控制' }
];

const accountBooks: AccountBook[] = [
  { code: 'CN-BOOK', name: '中国账套', currency: 'CNY', status: '启用' },
  { code: 'US-BOOK', name: '美国账套', currency: 'USD', status: '启用' },
  { code: 'EU-BOOK', name: '欧洲账套', currency: 'EUR', status: '停用' }
];

const STORAGE_KEY = 'foerp.default.accountbook';

export function App() {
  const [activeModule] = useState(modules[0]);
  const [selectedBookCode, setSelectedBookCode] = useState('CN-BOOK');
  const [defaultBookCode, setDefaultBookCode] = useState('CN-BOOK');

  useEffect(() => {
    const stored = window.localStorage.getItem(STORAGE_KEY);
    if (stored && accountBooks.some((book) => book.code === stored)) {
      setDefaultBookCode(stored);
      setSelectedBookCode(stored);
    }
  }, []);

  const selectedBook = useMemo(
    () => accountBooks.find((book) => book.code === selectedBookCode) ?? accountBooks[0],
    [selectedBookCode]
  );

  function saveDefaultBook() {
    setDefaultBookCode(selectedBookCode);
    window.localStorage.setItem(STORAGE_KEY, selectedBookCode);
  }

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
        <header className="topbar">
          <div>
            <h1>{activeModule.name}</h1>
            <p>{activeModule.capability}</p>
          </div>
          <div className="book-switcher">
            <label htmlFor="bookSwitcher">当前账套</label>
            <select
              id="bookSwitcher"
              value={selectedBookCode}
              onChange={(event) => setSelectedBookCode(event.target.value)}
            >
              {accountBooks
                .filter((book) => book.status === '启用')
                .map((book) => (
                  <option key={book.code} value={book.code}>
                    {book.name} ({book.code})
                  </option>
                ))}
            </select>
          </div>
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
                  <tr key={book.code} className={book.code === selectedBook.code ? 'row-active' : ''}>
                    <td>{book.code}</td>
                    <td>{book.name}</td>
                    <td>{book.currency}</td>
                    <td>{book.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </article>

          <article className="panel-card personal-settings">
            <h2>个人参数</h2>
            <p>可设置默认账套，登录后自动进入该账套。</p>
            <div className="settings-row">
              <label htmlFor="defaultBook">默认账套</label>
              <select
                id="defaultBook"
                value={selectedBookCode}
                onChange={(event) => setSelectedBookCode(event.target.value)}
              >
                {accountBooks
                  .filter((book) => book.status === '启用')
                  .map((book) => (
                    <option key={book.code} value={book.code}>
                      {book.name} ({book.code})
                    </option>
                  ))}
              </select>
              <button type="button" onClick={saveDefaultBook}>保存默认账套</button>
            </div>
            <p className="tip">当前默认账套：{defaultBookCode}</p>
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
