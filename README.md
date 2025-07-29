# 📚 JSON Article Website

[![roadmap.sh project](https://img.shields.io/badge/roadmap.sh-Personal%20Blog%20Project-blueviolet)](https://roadmap.sh/projects/personal-blog)

A lightweight, file-based article management system using JSON and HTML/Markdown. Designed for speed, simplicity, and minimal server dependencies. Ideal for static hosting, personal blogs, or offline publishing.

> 🛠️ This project is built as part of the [roadmap.sh Personal Blog Project](https://roadmap.sh/projects/personal-blog).

---

## 🚀 Features

- 📝 **Article Storage**: Articles are saved as `.json` files with HTML or Markdown content.
- 🔖 **Metadata Indexing**: Each article supports tags, categories, author, publish date, and description.
- 📊 **Statistics Support**: Track views, likes, comments, shares.
- 🔐 **Basic Authentication**: Optional JSON-based login for admin posting (no database required).
- 📁 **Pagination + Infinite Scroll**: Efficient file reading with lazy loading and page-wise loading.
- 🧠 **AI Enhancements (Planned)**: Auto-tagging, meta-description generation, NSFW detection, etc.
- 💬 **Comment API**: JSON-powered comment system (with future moderation tools).
- 🧩 **Extensible Architecture**: Easily customizable for SEO, feeds, sitemaps, themes, etc.

---

## 📂 Project Structure

```text
/
├── articles/
│   ├── 2025/
│   │   ├── 07/
│   │   │   ├── article-001.json
│   │   │   ├── article-002.json
│   │   │   └── ...
├── index.json             # All indexed metadata for fast listing/search
├── comments/
│   ├── article-001-comments.json
├── auth/
│   └── users.json         # Basic authentication credentials
├── stats/
│   └── views-likes.json   # Aggregated stats
├── tags.json              # All tags
├── categories.json        # All categories
├── assets/
│   └── images/, icons/, ...
└── README.md