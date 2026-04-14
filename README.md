# 🛒 Online Auction Platform

## 📌 Project Overview

The **Online Auction Platform** is a web-based system that enables users to participate in bidding for products. Sellers can list items for auction, while buyers can place bids. The platform automatically determines the highest bidder and notifies them when the auction ends.

This system is designed to be scalable, secure, and user-friendly, making it suitable for institutions, businesses, or independent sellers.

---

## 🎯 Objectives

* Enable seamless online bidding for users
* Automatically determine winning bids
* Notify winners and relevant participants
* Ensure secure authentication and transactions

---

## 👥 User Roles

### 1. Buyer / Bidder

* Register and log in
* Browse available auction items
* Place bids on items
* Receive bid updates
* Get notified if they win an auction

### 2. Seller / Auctioneer

* Create and manage auction listings
* Set starting price, minimum increment, and duration
* Monitor bids
* View auction results

### 3. Admin

* Manage users (buyers and sellers)
* Monitor platform activities
* Approve or remove auction listings
* Handle disputes and reports

---

## 🚀 Key Features

### 🔐 Authentication & Authorization

* Secure login and registration
* Role-based access control (Buyer, Seller, Admin)

### 📦 Auction Management

* Create, update, and delete auction items
* Set auction duration and rules
* Automatic auction closing

### 💰 Bidding System

* Bidding functionality
* Minimum bid increment enforcement
* Highest bid tracking

### 🔔 Notification System

* Notify highest bidder when auction ends
* Email or in-app notifications
* Alerts for being outbid

### 📊 Dashboard

* Buyers: Active bids, won auctions
* Sellers: Active listings, earnings
* Admin: System overview and analytics

---

## 🛠️ Tech Stack (Example)

### Frontend

* ASP .Net Web Application (Razor pages)
* Tailwind CSS

### Backend

* C#

### Database

* PostgreSQL

### Communication

* SMTP

### Authentication

* JWT (JSON Web Tokens)

---

## 🧩 System Architecture

* **Client (Frontend)** → User interface for buyers, sellers, admins
* **Server (Backend)** → Business logic, authentication, bidding engine
* **Database** → Stores users, auctions, bids, notifications
* **WebSocket Server** → Handles real-time bidding updates

---

## 🗄️ Database Design (Core Tables)

* Users (id, name, email, password, role)
* Auctions (id, title, description, start_price, end_time, seller_id)
* Bids (id, amount, user_id, auction_id, timestamp)
* Notifications (id, user_id, message, status)

---

## ⚙️ Installation & Setup

```bash
# Clone the repository
git clone https://github.com/your-username/auction-platform.git

# Navigate to the project directory
cd auction-platform

# Install dependencies
npm install

# Run the development server
npm run dev
```

---

## 🔄 How It Works

1. Seller creates an auction
2. Buyers place bids
3. System updates highest bid instantly
4. Auction ends automatically based on time
5. Highest bidder is declared the winner
6. Notification is sent to the winner

---

## 🔐 Security Considerations

* Password hashing (bcrypt)
* JWT-based authentication
* Input validation and sanitization
* Role-based access control
* Rate limiting on bidding

---

## 📬 Notifications Flow

* Outbid alert → Sent when another user places a higher bid
* Winning alert → Sent to highest bidder when auction ends
* Auction reminder → Sent before auction ends

---
