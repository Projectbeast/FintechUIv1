import { useState } from 'react';
import { 
  BarChart, 
  Bar, 
  LineChart, 
  Line, 
  XAxis, 
  YAxis, 
  CartesianGrid, 
  Tooltip, 
  ResponsiveContainer,
  AreaChart,
  Area,
  PieChart,
  Pie,
  Cell
} from 'recharts';
import { Calendar, TrendingUp, TrendingDown, DollarSign, Target } from 'lucide-react';

// Sample analytics data
const monthlyData = [
  { month: 'Jan', income: 8500, expenses: 6200, savings: 2300 },
  { month: 'Feb', income: 7800, expenses: 5800, savings: 2000 },
  { month: 'Mar', income: 9200, expenses: 6800, savings: 2400 },
  { month: 'Apr', income: 8800, expenses: 6400, savings: 2400 },
  { month: 'May', income: 9500, expenses: 7100, savings: 2400 },
  { month: 'Jun', income: 8500, expenses: 6250, savings: 2250 },
];

const categorySpending = [
  { category: 'Housing', amount: 2400, percentage: 35 },
  { category: 'Food', amount: 800, percentage: 12 },
  { category: 'Transportation', amount: 600, percentage: 9 },
  { category: 'Entertainment', amount: 400, percentage: 6 },
  { category: 'Healthcare', amount: 300, percentage: 4 },
  { category: 'Utilities', amount: 350, percentage: 5 },
  { category: 'Others', amount: 1400, percentage: 20 },
];

const investmentData = [
  { month: 'Jan', stocks: 15000, bonds: 8000, crypto: 2000 },
  { month: 'Feb', stocks: 15800, bonds: 8200, crypto: 1800 },
  { month: 'Mar', stocks: 16200, bonds: 8100, crypto: 2200 },
  { month: 'Apr', stocks: 15900, bonds: 8300, crypto: 2100 },
  { month: 'May', stocks: 16800, bonds: 8400, crypto: 2400 },
  { month: 'Jun', stocks: 17200, bonds: 8500, crypto: 2200 },
];

const savingsGoalData = [
  { goal: 'Emergency Fund', target: 15000, current: 12500 },
  { goal: 'Vacation', target: 5000, current: 3200 },
  { goal: 'New Car', target: 25000, current: 8700 },
  { goal: 'Home Down Payment', target: 50000, current: 18500 },
];

const COLORS = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#06b6d4', '#84cc16'];

const Analytics = () => {
  const [timeRange, setTimeRange] = useState('6m');

  return (
    <div className="space-y-6">
      {/* Header with Time Range Selector */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h2 className="text-2xl font-semibold text-gray-900">Financial Analytics</h2>
          <p className="text-gray-600 mt-1">Track your financial performance and insights</p>
        </div>
        <div className="flex items-center space-x-2">
          <Calendar className="h-5 w-5 text-gray-400" />
          <select
            value={timeRange}
            onChange={(e) => setTimeRange(e.target.value)}
            className="px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="1m">Last Month</option>
            <option value="3m">Last 3 Months</option>
            <option value="6m">Last 6 Months</option>
            <option value="1y">Last Year</option>
          </select>
        </div>
      </div>

      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <div className="flex items-center">
            <div className="p-2 bg-green-100 rounded-lg mr-3">
              <TrendingUp className="h-6 w-6 text-green-600" />
            </div>
            <div>
              <p className="text-sm text-gray-600">Avg Monthly Income</p>
              <p className="text-xl font-semibold text-green-600">$8,883</p>
              <p className="text-xs text-green-600">+5.2% vs last period</p>
            </div>
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <div className="flex items-center">
            <div className="p-2 bg-red-100 rounded-lg mr-3">
              <TrendingDown className="h-6 w-6 text-red-600" />
            </div>
            <div>
              <p className="text-sm text-gray-600">Avg Monthly Expenses</p>
              <p className="text-xl font-semibold text-red-600">$6,425</p>
              <p className="text-xs text-red-600">-2.1% vs last period</p>
            </div>
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <div className="flex items-center">
            <div className="p-2 bg-blue-100 rounded-lg mr-3">
              <DollarSign className="h-6 w-6 text-blue-600" />
            </div>
            <div>
              <p className="text-sm text-gray-600">Savings Rate</p>
              <p className="text-xl font-semibold text-blue-600">27.2%</p>
              <p className="text-xs text-blue-600">+1.8% vs last period</p>
            </div>
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <div className="flex items-center">
            <div className="p-2 bg-purple-100 rounded-lg mr-3">
              <Target className="h-6 w-6 text-purple-600" />
            </div>
            <div>
              <p className="text-sm text-gray-600">Goal Progress</p>
              <p className="text-xl font-semibold text-purple-600">68%</p>
              <p className="text-xs text-purple-600">On track</p>
            </div>
          </div>
        </div>
      </div>

      {/* Charts Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Income vs Expenses */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Income vs Expenses</h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={monthlyData}>
              <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
              <XAxis dataKey="month" stroke="#6b7280" />
              <YAxis stroke="#6b7280" />
              <Tooltip 
                formatter={(value: any, name: string) => [`$${value.toLocaleString()}`, name.charAt(0).toUpperCase() + name.slice(1)]}
              />
              <Bar dataKey="income" fill="#10b981" radius={[2, 2, 0, 0]} />
              <Bar dataKey="expenses" fill="#ef4444" radius={[2, 2, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Spending by Category */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Spending by Category</h3>
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={categorySpending}
                cx="50%"
                cy="50%"
                innerRadius={60}
                outerRadius={120}
                paddingAngle={2}
                dataKey="amount"
              >
                {categorySpending.map((_, index) => (
                  <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip formatter={(value: any) => `$${value.toLocaleString()}`} />
            </PieChart>
          </ResponsiveContainer>
        </div>

        {/* Investment Portfolio */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Investment Portfolio</h3>
          <ResponsiveContainer width="100%" height={300}>
            <AreaChart data={investmentData}>
              <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
              <XAxis dataKey="month" stroke="#6b7280" />
              <YAxis stroke="#6b7280" />
              <Tooltip 
                formatter={(value: any, name: string) => [`$${value.toLocaleString()}`, name.charAt(0).toUpperCase() + name.slice(1)]}
              />
              <Area type="monotone" dataKey="stocks" stackId="1" stroke="#3b82f6" fill="#3b82f6" />
              <Area type="monotone" dataKey="bonds" stackId="1" stroke="#10b981" fill="#10b981" />
              <Area type="monotone" dataKey="crypto" stackId="1" stroke="#f59e0b" fill="#f59e0b" />
            </AreaChart>
          </ResponsiveContainer>
        </div>

        {/* Savings Trend */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Savings Trend</h3>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={monthlyData}>
              <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
              <XAxis dataKey="month" stroke="#6b7280" />
              <YAxis stroke="#6b7280" />
              <Tooltip 
                formatter={(value: any) => [`$${value.toLocaleString()}`, 'Savings']}
              />
              <Line 
                type="monotone" 
                dataKey="savings" 
                stroke="#8b5cf6" 
                strokeWidth={3}
                dot={{ fill: '#8b5cf6', r: 6 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Goals Progress */}
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
        <h3 className="text-lg font-semibold text-gray-900 mb-6">Savings Goals Progress</h3>
        <div className="space-y-6">
          {savingsGoalData.map((goal, index) => {
            const progress = (goal.current / goal.target) * 100;
            return (
              <div key={index}>
                <div className="flex justify-between items-center mb-2">
                  <span className="font-medium text-gray-900">{goal.goal}</span>
                  <span className="text-sm text-gray-600">
                    ${goal.current.toLocaleString()} / ${goal.target.toLocaleString()}
                  </span>
                </div>
                <div className="w-full bg-gray-200 rounded-full h-3">
                  <div
                    className="bg-blue-600 h-3 rounded-full transition-all duration-300"
                    style={{ width: `${Math.min(progress, 100)}%` }}
                  ></div>
                </div>
                <div className="text-right mt-1">
                  <span className="text-sm font-medium text-blue-600">
                    {progress.toFixed(1)}%
                  </span>
                </div>
              </div>
            );
          })}
        </div>
      </div>

      {/* Category Breakdown Table */}
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
        <h3 className="text-lg font-semibold text-gray-900 mb-4">Category Breakdown</h3>
        <div className="overflow-x-auto">
          <table className="min-w-full">
            <thead>
              <tr className="border-b border-gray-200">
                <th className="text-left py-3 px-4 font-semibold text-gray-900">Category</th>
                <th className="text-left py-3 px-4 font-semibold text-gray-900">Amount</th>
                <th className="text-left py-3 px-4 font-semibold text-gray-900">Percentage</th>
                <th className="text-left py-3 px-4 font-semibold text-gray-900">Trend</th>
              </tr>
            </thead>
            <tbody>
              {categorySpending.map((category, index) => (
                <tr key={index} className="border-b border-gray-100">
                  <td className="py-3 px-4 font-medium text-gray-900">{category.category}</td>
                  <td className="py-3 px-4 text-gray-600">${category.amount.toLocaleString()}</td>
                  <td className="py-3 px-4">
                    <div className="flex items-center">
                      <div className="w-16 bg-gray-200 rounded-full h-2 mr-2">
                        <div
                          className="bg-blue-600 h-2 rounded-full"
                          style={{ width: `${category.percentage}%` }}
                        ></div>
                      </div>
                      <span className="text-sm text-gray-600">{category.percentage}%</span>
                    </div>
                  </td>
                  <td className="py-3 px-4">
                    <span className={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${
                      index % 2 === 0 ? 'text-green-600 bg-green-100' : 'text-red-600 bg-red-100'
                    }`}>
                      {index % 2 === 0 ? '↓ 2.1%' : '↑ 1.5%'}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Analytics;