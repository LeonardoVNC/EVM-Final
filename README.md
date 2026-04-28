# Postgrado Night

> Survival horror en primera persona ambientado en las instalaciones reales de la universidad. ¿Puedes sobrevivir hasta las 6:00 AM?

---

##  Descripción

**Postgrado Night** es un juego de survival horror inspirado en *Five Nights at Freddy's*. El jugador asume el rol de un becario universitario  que debe sobrevivir una noche completa (12:00 AM → 6:00 AM) gestionando recursos limitados, vigilando cámaras de seguridad y reaccionando a la amenaza de 4 animatrónicos autónomos con comportamientos únicos.

- **Condición de victoria:** el reloj llega a las 6:00 AM con el jugador vivo.
- **Condición de derrota:** batería al 0% **o** un animatrónico logra entrar al cuarto del guardia.

---

## 🕹️ Mecánicas Principales

### Controles

| Tecla / Input | Acción | Nota |
|---|---|---|
| Tecla `espacio` | Abrir/cerrar panel de cámaras | Consume batería mientras está abierto |
| Click izquierdo | Activar linterna | Consume batería mientras está activa |
| `E` | Cerrar la puerta cuando miras el boton rojo de la puerta | Consume batería por puerta cerrada |
| Mouse | Mirar alrededor en el cuarto | Ángulo limitado, vista en primera persona |
| Click izquierdo | seleccionar la camara para ver el cuarto | cuando estas en la vista de camara |

### Sistema de Batería

La batería es el recurso central del juego. Llegar a 0% significa muerte inmediata.

| Acción | Consumo |
|---|---|
| Panel de cámaras abierto | Alto — continuo |
| Linterna encendida | Medio — continuo |
| Puertas cerradas | Medio — continuo por puerta |
| Sin acciones activas | Mínimo |



### Los 4 Animatrónicos

| Animatrónico | Activación | Comportamiento | Cómo detenerlo |
|---|---|---|---|
| **A1 — El Rondador** | 1:00 AM | Recorre pasillos del piso 1 en ruta fija. Emite sonido de pasos. | Cerrar la puerta antes de que llegue. |

*Falta implementar*

| Animatrónico | Activación | Comportamiento | Cómo detenerlo |
|---|---|---|---|
| **A2 — El Agresivo** | 2:00 AM | Más rápido que A1. Toma ruta por cafetería/garaje. | Cerrar la puerta del lado contrario a A1. |
| **A3 — El Espectro** | 3:00 AM | Se teletransporta al cuarto y se oculta en una esquina (~cada 90s). | Alumbrar con linterna en menos de 3 segundos. |
| **A4 — El Controlado** | 4:00 AM+ | Ataca si el jugador suelta el botón de control por más de 2 segundos. | Mantener presionado el botón en pantalla. |

### Sistema de Cámaras

El jugador dispone de hasta 10 cámaras de seguridad distribuidas en ambos pisos del edificio. Mantener el panel activo consume batería de forma continua.

| Cámara | Ubicación | Importancia |
|---|---|---|
| CAM 01 | Cafetería — Piso 1 | Zona de inicio de A2 |
| CAM 02 | Auditorio — Piso 1 | Zona de inicio de A1 |
| CAM 03 | Baños — Piso 1 | Punto de paso de A1 |
| CAM 04 | Pasillo — Piso 1 | Ruta principal de ambos |

*Falta implementar*

| Cámara | Ubicación | Importancia |
|---|---|---|
| CAM 05 | Garaje (exterior) | Entrada alternativa de A2 |
| CAM 06 | Salida Universidad | Control de perímetro |
| CAM 07 | Gradas/Escaleras | Detectar si sube al piso 2 |
| CAM 08 | Pasillo — Piso 2 | Movimiento en piso 2 |
| CAM 09 | Aula 1 — Piso 2 | Zona de spawn de A3 |
| CAM 10 | Aula 2 — Piso 2 | Zona secundaria |

---

## Tecnologías Utilizadas

| Tecnología | Uso |
|---|---|
| **Unity 3D** | Motor principal de desarrollo |
| **C#** | Lenguaje de programación para todos los scripts |
| **Unity NavMesh / Waypoints** | Pathfinding y movimiento de animatrónicos |
| **Unity Audio System** | Sonido ambiente, pasos, screamers y música |
| **Unity UI (Canvas)** | HUD: batería, reloj, panel de cámaras, botón A4 |
| **Unity Animator Controller** | Gestión de animaciones y transiciones de los animatrónicos |
---

## Jerarquía de la Escena

```
Night1
├── Global Volume
├── Managers
│   ├── GameManager
│   ├── CameraManager
│   │   ├── CafeteriaCam
│   │   ├── AuditorioCam
│   │   ├── BanioF1Cam
│   │   └── HallF1Cam
│   └── GameTimeManager
├── Player
│   └── Main Camera
│       └── Flashlight
├── Animatronics
├── Canvas
│   ├── SecurityUI
│   │   ├── Effects
│   │   │   ├── StaticImage
│   │   │   └── ScreenImage
│   │   └── CamPanel
│   │       ├── CafeteriaButton
│   │       ├── AuditorioButton
│   │       ├── BanioF1Button
│   │       └── HallF1Button
│   └── GameUI
│       ├── TimeText
│       ├── BatteryUI
│       └── FlashlightIcon
├── Primer_piso
├── Segundo_piso_b
└── Others
    ├── EventSystem
    ├── StaticVideoPlayer
    ├── DoorController_Izquierdo
    └── DoorController_Derecho
```

---

## 🧩 Patrones de Diseño

### Singleton — `GlobalAudioManager`

El `GlobalAudioManager` está implementado como **Singleton** en la escena del Main Menu. Recibe `AudioClip`s y los reproduce a través de un `AudioSource` centralizado, garantizando que exista una única instancia del gestor de audio activa en todo momento y que sea accesible globalmente desde cualquier script del proyecto.

```csharp
// Ejemplo de uso del patrón Singleton
GlobalAudioManager.Instance.Play(clip);
```

### State Machine — `FogManager`

El `FogManager` en `Night1` usa un **State Pattern** para controlar la intensidad de la niebla del entorno. Cada estado representa un nivel de densidad de niebla distinto, y las transiciones entre estados se disparan según los eventos del juego (hora actual, proximidad de animatrónicos, etc.). Esto permite que el ambiente visual escale con la tensión de la partida sin lógica condicional anidada.

```
Estados de FogManager:
  ├── Estado: Sin niebla      → inicio de la noche
  ├── Estado: Niebla leve     → primeras horas
  └── Estado: Niebla densa    → madrugada / alta tensión
```

### State Machine Visual — `Animator Controller`

Unity's `Animator Controller` implementa internamente una máquina de estados para gestionar las animaciones de los animatrónicos. Cada estado representa una animación (idle, caminar, atacar) y las transiciones entre ellos se disparan mediante parámetros controlados desde los scripts de IA (`AnimatronicAI.cs` y sus subclases), conectando directamente el comportamiento lógico del animatrónico con su representación visual.

---

## ▶️ Instalación y Ejecución

### Requisitos

- **Unity 6.3 LTS**
- Sistema operativo: **Windows** (plataforma objetivo: PC)

### Pasos para ejecutar

1. Clonar o descargar el repositorio del proyecto.
2. Abrir **Unity Hub** y seleccionar **"Add project from disk"**.
3. Navegar a la carpeta raíz del proyecto y abrirla.
4. Una vez cargado el proyecto en Unity, abrir la escena `MainMenu` desde la carpeta `Assets/Scenes/`.
5. Presionar el botón **▶ Play** en el editor, o generar un build desde `File → Build Settings → Build and Run`.

---
