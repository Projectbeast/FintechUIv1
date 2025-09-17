# FintechUIv1

A modern, responsive fintech dashboard built with React, TypeScript, and Tailwind CSS. This application provides comprehensive financial management tools including dashboard analytics, transaction management, and detailed financial insights.

## Features

### 📊 Dashboard
- Real-time financial metrics display
- Account balance trends with interactive charts
- Monthly income and expense tracking
- Investment return monitoring
- Recent transactions overview
- Expense breakdown by category (pie chart)

### 💳 Transaction Management
- Comprehensive transaction listing with search and filtering
- Transaction categorization (Income, Expense, Transfer)
- Status tracking (Completed, Pending, Failed)
- Financial summary cards (Total Income, Expenses, Net Flow)
- Export functionality for financial records

### 📈 Analytics & Insights
- Financial performance analytics with multiple chart types
- Income vs Expenses comparison (bar charts)
- Investment portfolio tracking (area charts)
- Savings trend analysis (line charts)
- Spending category breakdown with percentages
- Savings goals progress tracking
- Time range filtering (Last Month, 3 Months, 6 Months, Year)

## Technology Stack

- **Frontend Framework**: React 18 with TypeScript
- **Build Tool**: Vite
- **Styling**: Tailwind CSS
- **Charts**: Recharts library for data visualization
- **Icons**: Lucide React icons
- **Routing**: React Router DOM
- **UI Components**: Custom responsive components

## Getting Started

### Prerequisites
- Node.js (version 16 or higher)
- npm or yarn package manager

### Installation

1. Clone the repository:
```bash
git clone https://github.com/Projectbeast/FintechUIv1.git
cd FintechUIv1
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm run dev
```

4. Open your browser and navigate to `http://localhost:5173`

### Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

## Project Structure

```
src/
├── components/
│   ├── layout/
│   │   └── Layout.tsx          # Main layout with sidebar navigation
│   ├── dashboard/
│   │   └── Dashboard.tsx       # Dashboard with metrics and charts
│   ├── transactions/
│   │   └── Transactions.tsx    # Transaction management interface
│   └── charts/
│       └── Analytics.tsx       # Analytics and insights page
├── types/
│   └── index.ts               # TypeScript type definitions
├── App.tsx                    # Main application component
├── main.tsx                   # Application entry point
└── index.css                  # Global styles with Tailwind

```

## Key Features Implementation

### Responsive Design
- Mobile-first approach with responsive sidebar navigation
- Adaptive grid layouts for different screen sizes
- Touch-friendly interface elements

### Data Visualization
- Interactive charts with hover tooltips
- Multiple chart types: Line, Bar, Pie, and Area charts
- Color-coded financial data for better understanding

### User Experience
- Clean, modern interface design
- Intuitive navigation between different sections
- Real-time data updates and smooth animations
- Accessible UI components with proper ARIA labels

### Financial Metrics
- Balance tracking with trend indicators
- Expense categorization and analysis
- Investment portfolio monitoring
- Goal progress tracking with visual indicators

## Sample Data

The application includes comprehensive sample financial data demonstrating:
- Various transaction types and categories
- Historical balance and investment data
- Multiple expense categories with realistic amounts
- Savings goals with progress tracking

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Screenshots

### Dashboard
![Dashboard](https://github.com/user-attachments/assets/05b9ceb2-71dc-480f-b14e-65793ac92a1d)

The dashboard provides a comprehensive overview of financial metrics with interactive charts and real-time data visualization.

---

*Built with ❤️ using modern web technologies for the fintech industry*
