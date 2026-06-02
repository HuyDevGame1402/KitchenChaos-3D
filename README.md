# 🍳 KitchenChaos — 3D Cooking Game

![Unity](https://img.shields.io/badge/Unity-2022.3_LTS-black?logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-purple)
![Pipeline](https://img.shields.io/badge/Render-URP-blue)
![Platform](https://img.shields.io/badge/Platform-PC-lightgrey)

Tựa game nấu ăn 3D nhập vai người phụ bếp trong một nhà hàng hỗn loạn. Người chơi phải di chuyển, nhặt nguyên liệu, chế biến và phục vụ các món ăn đúng thời hạn trước khi khách hàng mất kiên nhẫn. Lấy cảm hứng từ phong cách **Overcooked**, game được xây dựng bằng **Unity 3D** với kiến trúc code sạch theo **ScriptableObject** và **Design Patterns**.

---

## 🛠️ Công Nghệ Sử Dụng (Tech Stack)

| Công cụ | Phiên bản | Vai trò |
|---|---|---|
| **Unity** | 2022.3 LTS | Engine phát triển game chính |
| **C#** | .NET | Ngôn ngữ lập trình chính |
| **Input System** | New Input System | Xử lý input bàn phím / gamepad |
| **ScriptableObject** | — | Quản lý dữ liệu món ăn, công thức nấu |

---

## 📌 Tính Năng Nổi Bật (Features)

### 🧑‍🍳 Gameplay Nấu Ăn
- Nhặt, mang và đặt nguyên liệu lên các bàn chế biến
- Thái, nướng, chiên nguyên liệu đúng thời gian — để quá lửa sẽ cháy
- Kết hợp nhiều nguyên liệu theo đúng công thức để tạo món hoàn chỉnh
- Phục vụ món ăn đúng yêu cầu của khách trước khi hết giờ

### 📋 Hệ Thống Đơn Hàng (Order System)
- Đơn hàng xuất hiện ngẫu nhiên theo thời gian thực
- Mỗi đơn có bộ đếm thời gian riêng — hết giờ bị trừ điểm
- Hiển thị trực quan yêu cầu món ăn trên UI

### 🍳 Hệ Thống Chế Biến (Cooking System)
- **Thái (Cutting):** Giữ tương tác liên tục để thái nguyên liệu
- **Nấu (Cooking):** Đặt lên bếp, theo dõi thanh tiến trình nấu
- **Cháy (Burning):** Để quá lâu → nguyên liệu cháy → phải vứt đi
- Dữ liệu công thức quản lý hoàn toàn qua **ScriptableObject**

### 🎮 Input System
- Hỗ trợ bàn phím và gamepad qua **New Input System** của Unity
- Cấu hình linh hoạt qua `InputAction` asset

### 🔊 Âm Thanh & Hiệu Ứng
- Âm thanh thái đồ, tiếng xèo chảo, tiếng chuông đơn hàng
- Hiệu ứng hạt (Particle) khi nấu xong, khi cháy

---

## 📁 Cấu Trúc Thư Mục (Assets Structure)

```text
Assets/
│
├── 🎮 Gameplay & Logic
│   ├── Scirpts/                 # Toàn bộ mã nguồn C# (xem chi tiết bên dưới)
│   ├── Prefabs/                 # Prefab: nhân vật, bàn bếp, nguyên liệu, UI elements
│   └── ScirptableObjects/       # ScriptableObject: công thức món ăn, dữ liệu nguyên liệu
│
├── 🎬 Scene
│   └── Scenes/                  # Các Scene: MainMenu, GameScene, GameOver
│
├── 🕹️ Input
│   └── InputAction/             # Input Action Asset cấu hình bàn phím / gamepad
│
├── 🔧 Config & Settings
│   ├── Settings/                # URP Settings, Audio Mixer, Quality Settings
│   └── TutorialInfo/            # Tài liệu tham khảo Asset bên thứ ba
│
└── 📦 Third Party
    └── CodeMonkeyFree/          # Utilities & Helper Scripts từ CodeMonkey (UI, Events)
```

---

## 🛠️ Kiến Trúc Mã Nguồn (Scripts Architecture)

### 🧑‍🍳 Player & Interaction
| Script | Chức năng |
|---|---|
| `PlayerController.cs` | Di chuyển nhân vật 3D, xoay hướng theo input |
| `PlayerInteract.cs` | Phát hiện và tương tác với các Counter xung quanh |
| `PlayerAnimator.cs` | Điều khiển Animator theo trạng thái di chuyển / tương tác |

### 🍽️ Counter System — Bàn Chế Biến
| Script | Chức năng |
|---|---|
| `BaseCounter.cs` | Lớp cha chung cho tất cả các loại bàn |
| `ClearCounter.cs` | Bàn trống: đặt / lấy nguyên liệu |
| `CuttingCounter.cs` | Bàn thái: xử lý logic thái nguyên liệu, thanh tiến trình |
| `StoveCounter.cs` | Bếp nấu: nấu nguyên liệu, theo dõi trạng thái chín / cháy |
| `PlateCounter.cs` | Bàn lấy đĩa sạch để phục vụ |
| `DeliveryCounter.cs` | Bàn nộp món: kiểm tra đơn hàng và tính điểm |
| `TrashCounter.cs` | Thùng rác: vứt nguyên liệu hỏng / cháy |
| `ContainerCounter.cs` | Thùng chứa nguyên liệu đầu vào |

### 📋 Order & Score System
| Script | Chức năng |
|---|---|
| `DeliveryManager.cs` | Sinh đơn hàng ngẫu nhiên, quản lý danh sách chờ, đếm giờ |
| `OrderUI.cs` | Hiển thị danh sách đơn hàng và thời gian còn lại lên HUD |
| `ScoreManager.cs` | Tính điểm khi giao đúng món, trừ điểm khi đơn hết hạn |

### 🍳 Cooking & Recipe
| Script | Chức năng |
|---|---|
| `CuttingRecipeSO.cs` | ScriptableObject: định nghĩa nguyên liệu đầu vào → đầu ra khi thái |
| `CookingRecipeSO.cs` | ScriptableObject: định nghĩa nguyên liệu → kết quả nấu chín / cháy |
| `RecipeListSO.cs` | ScriptableObject: danh sách tất cả công thức món hoàn chỉnh |
| `KitchenObjectSO.cs` | ScriptableObject: dữ liệu từng nguyên liệu (tên, sprite, prefab) |

### ⏱️ Game Flow
| Script | Chức năng |
|---|---|
| `GameManager.cs` | Quản lý trạng thái: WaitingToStart → Playing → GameOver |
| `GameTimerUI.cs` | Đếm ngược thời gian một màn chơi, hiển thị lên HUD |
| `GameOverUI.cs` | Màn hình kết thúc: tổng điểm, số đơn hoàn thành, nút chơi lại |

### 🔊 Audio & VFX
| Script | Chức năng |
|---|---|
| `SoundManager.cs` | Phát SFX: thái đồ, xèo chảo, giao món, cháy, chuông đơn hàng |
| `MusicManager.cs` | Phát nhạc nền theo trạng thái game |

---

## 🕹️ Điều Khiển (Controls)

| Phím | Hành động |
|---|---|
| `W` `A` `S` `D` | Di chuyển nhân vật |
| `E` / `Space` | Tương tác với bàn / nhặt đồ |
| `F` | Ném / vứt vật đang cầm |
| **Gamepad** | Hỗ trợ đầy đủ qua New Input System |

---

## 🚀 Hướng Dẫn Cài Đặt (Installation & Setup)

### 🖥️ Yêu Cầu Hệ Thống (Prerequisites)
- **Unity Editor:** `2022.3 LTS` trở lên
- **Input System Package:** Đã cấu hình sẵn trong `InputAction/`

### 📋 Các Bước Thực Hiện

**1. Clone mã nguồn từ GitHub**
```bash
git clone https://github.com/HuyDevGame1402/KitchenChaos.git
```

**2. Mở dự án bằng Unity Hub**
- Mở **Unity Hub** → **Add → Add project from disk**
- Chọn phiên bản `2022.3 LTS` và mở dự án

**3. Kiểm tra Input System**
- Vào **Edit → Project Settings → Player**
- Đảm bảo **Active Input Handling** được đặt là `Input System Package (New)`

**4. Mở Scene và chạy game**
- Trong cửa sổ **Project**, mở `Assets/Scenes/GameScene.unity`
- Nhấn **▶ Play** để bắt đầu

---

## 📝 Bản Quyền & Ghi Chú (License & Notes)

- **Trạng thái:** Hoàn thành Core Gameplay Loop
- **CodeMonkeyFree** — Utilities từ [Code Monkey](https://unitycodemonkey.com), thuộc bản quyền tác giả
- **Mã nguồn Scripts** được phát triển nội bộ bởi tác giả

> 💡 Dự án xây dựng theo hướng dẫn của **Code Monkey** trên YouTube — tập trung vào thực hành **Clean Code**, **ScriptableObject Architecture** và **Event-Driven Design** trong Unity.

---

## 👤 Tác Giả (Author)

| | |
|---|---|
| **Họ và Tên** | Nguyễn Đức Huy |
| **Email** | [huyco14022004@gmail.com](mailto:huyco14022004@gmail.com) |
| **GitHub** | [HuyDevGame1402](https://github.com/HuyDevGame1402) |
| **LinkedIn** | [nguyễn-đức-huy](https://www.linkedin.com/in/nguy%E1%BB%85n-%C4%91%E1%BB%A9c-huy-081a73411/) |
