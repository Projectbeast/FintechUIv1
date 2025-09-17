import { DollarSign, TrendingUp, TrendingDown, CreditCard, PiggyBank, ArrowUpRight, ArrowDownRight } from 'lucide-react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';

interface Transaction {
  id: string;
  date: string;
  amount: number;
  type: 'income' | 'expense' | 'transfer';
  category: string;
  description: string;
  status: 'completed' | 'pending' | 'failed';
}

// Sample data
const stats = {
  totalBalance: 45678.90,
  monthlyIncome: 8500.00,
  monthlyExpenses: 3250.75,
  investmentReturn: 12.5,
};

const recentTransactions: Transaction[] = [
  {
    id: '1',
    date: '2024-01-15',
    amount: -85.20,
    type: 'expense',
    category: 'Groceries',
    description: 'Whole Foods Market',
    status: 'completed',
  },
  {
    id: '2',
    date: '2024-01-14',
    amount: 2500.00,
    type: 'income',
    category: 'Salary',
    description: 'Monthly Salary',
    status: 'completed',
  },
  {
    id: '3',
    date: '2024-01-13',
    amount: -45.99,
    type: 'expense',
    category: 'Utilities',
    description: 'Internet Bill',
    status: 'completed',
  },
  {
    id: '4',
    date: '2024-01-12',
    amount: -120.00,
    type: 'expense',
    category: 'Transportation',
    description: 'Gas Station',
    status: 'pending',
  },
];

const accountBalanceData = [
  { month: 'Jan', balance: 42000 },
  { month: 'Feb', balance: 43500 },
  { month: 'Mar', balance: 41000 },
  { month: 'Apr', balance: 44200 },
  { month: 'May', balance: 45000 },
  { month: 'Jun', balance: 45678 },
];

const expenseData = [
  { category: 'Housing', amount: 1200, color: '#3b82f6' },
  { category: 'Food', amount: 800, color: '#10b981' },
  { category: 'Transportation', amount: 400, color: '#f59e0b' },
  { category: 'Entertainment', amount: 300, color: '#ef4444' },
  { category: 'Others', amount: 550, color: '#8b5cf6' },
];

const StatCard = ({ icon: Icon, title, value, change, changeType }: {
  icon: any;
  title: string;
  value: string;
  change: string;
  changeType: 'positive' | 'negative' | 'neutral';
}) => {
  const changeColor = changeType === 'positive' ? 'text-green-600' : 
                     changeType === 'negative' ? 'text-red-600' : 'text-gray-600';
  const ChangeIcon = changeType === 'positive' ? ArrowUpRight : 
                     changeType === 'negative' ? ArrowDownRight : null;

  return (
    <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm font-medium text-gray-600">{title}</p>
          <p className="text-2xl font-semibold text-gray-900 mt-2">{value}</p>
          <div className={`flex items-center mt-1 text-sm ${changeColor}`}>
            {ChangeIcon && <ChangeIcon className="h-4 w-4 mr-1" />}
            {change}
          </div>
        </div>
        <div className="p-3 bg-blue-50 rounded-full">
          <Icon className="h-6 w-6 text-blue-600" />
        </div>
      </div>
    </div>
  );
};

const Dashboard = () => {
  return (
    <div className="space-y-6">
      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard
          icon={DollarSign}
          title="Total Balance"
          value={`$${stats.totalBalance.toLocaleString()}`}
          change="+5.2%"
          changeType="positive"
        />
        <StatCard
          icon={TrendingUp}
          title="Monthly Income"
          value={`$${stats.monthlyIncome.toLocaleString()}`}
          change="+8.1%"
          changeType="positive"
        />
        <StatCard
          icon={TrendingDown}
          title="Monthly Expenses"
          value={`$${stats.monthlyExpenses.toLocaleString()}`}
          change="-2.4%"
          changeType="positive"
        />
        <StatCard
          icon={PiggyBank}
          title="Investment Return"
          value={`${stats.investmentReturn}%`}
          change="+1.2%"
          changeType="positive"
        />
      </div>

      {/* Charts Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Account Balance Trend */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Account Balance Trend</h3>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={accountBalanceData}>
              <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
              <XAxis dataKey="month" stroke="#6b7280" />
              <YAxis stroke="#6b7280" />
              <Tooltip 
                formatter={(value: any) => [`$${value.toLocaleString()}`, 'Balance']}
                labelStyle={{ color: '#374151' }}
              />
              <Line 
                type="monotone" 
                dataKey="balance" 
                stroke="#3b82f6" 
                strokeWidth={3}
                dot={{ fill: '#3b82f6', r: 6 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>

        {/* Expense Breakdown */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Monthly Expenses</h3>
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={expenseData}
                cx="50%"
                cy="50%"
                innerRadius={60}
                outerRadius={120}
                paddingAngle={2}
                dataKey="amount"
              >
                {expenseData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip formatter={(value: any) => `$${value}`} />
            </PieChart>
          </ResponsiveContainer>
          <div className="grid grid-cols-2 gap-2 mt-4">
            {expenseData.map((item, index) => (
              <div key={index} className="flex items-center">
                <div 
                  className="w-3 h-3 rounded-full mr-2" 
                  style={{ backgroundColor: item.color }}
                ></div>
                <span className="text-sm text-gray-600">{item.category}</span>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Recent Transactions */}
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
        <div className="flex items-center justify-between mb-4">
          <h3 className="text-lg font-semibold text-gray-900">Recent Transactions</h3>
          <button className="text-blue-600 hover:text-blue-700 font-medium">
            View All
          </button>
        </div>
        <div className="space-y-4">
          {recentTransactions.map((transaction) => (
            <div key={transaction.id} className="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
              <div className="flex items-center">
                <div className={`p-2 rounded-full mr-3 ${
                  transaction.type === 'income' ? 'bg-green-100' : 'bg-red-100'
                }`}>
                  <CreditCard className={`h-5 w-5 ${
                    transaction.type === 'income' ? 'text-green-600' : 'text-red-600'
                  }`} />
                </div>
                <div>
                  <p className="font-medium text-gray-900">{transaction.description}</p>
                  <p className="text-sm text-gray-600">{transaction.category}</p>
                </div>
              </div>
              <div className="text-right">
                <p className={`font-semibold ${
                  transaction.amount > 0 ? 'text-green-600' : 'text-red-600'
                }`}>
                  {transaction.amount > 0 ? '+' : ''}${Math.abs(transaction.amount).toFixed(2)}
                </p>
                <p className="text-sm text-gray-600">{transaction.date}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Dashboard;