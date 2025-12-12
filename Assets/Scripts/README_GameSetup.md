# Setup rápido para el minijuego VR (pelotas y objetivo)

Estos son los pasos para configurar la escena en Unity usando los scripts añadidos:

1) Crear el prefab de la bola
   - Crear una esfera (GameObject > 3D Object > Sphere).
   - Añadir `Rigidbody` (use masa ~1, drag 0.1, angular drag 0.05).
   - Añadir el script `Ball` (Assets/Scripts/Ball.cs).
   - (Opcional, recomendado) Añadir `XR Grab Interactable` si usas XR Interaction Toolkit para poder agarrarla.
   - Guardar como prefab en `Assets/Prefabs/Ball.prefab`.

2) Configurar el spawner
   - Crear un GameObject vacío `BallSpawner` y añadir el script `BallSpawner`.
   - Asignar la referencia `ballPrefab` al prefab creado.
   - Ajustar `spawnInterval`, `spawnAreaSize` y `spawnHeight` a tu escena.

3) Crear el objetivo en movimiento
   - Crear un GameObject (por ejemplo un cubo) y añadir `MovingTarget`.
   - Poner su Tag a `Target` (importante para detección de colisión).
   - Asegurarse que tenga Collider (no marcado como Trigger) para recibir colisiones.

4) Añadir GameManager y UI
   - Crear un GameObject `GameManager` y añadir el script `GameManager`.
   - Crear un Canvas (World Space o Screen Space) y añadir dos `TextMeshPro - Text (UI)` para score y timer.
   - Crear un GameObject `UIManager` y añadir el script `UIManager`, enlazar los campos `scoreText` y `timerText` a los TMP objects.

5) Jugabilidad
   - Ejecuta la escena. Las pelotas caerán desde arriba. Agárralas (con XR grab) y lánzalas hacia el objetivo.
   - Si la bola choca con el objeto con suficiente velocidad, se suman 100 puntos.
   - El temporizador cuenta desde 3 minutos (180 segundos) y al terminar el juego deja de sumar.

6) Panel de fin de juego (Restart / Quit)
   - Crea un Panel bajo tu Canvas llamado `EndGamePanel` y dentro añade dos `Button` con texto "Reiniciar" y "Salir".
   - Añade el script `EndGameUI` (Assets/Scripts/EndGameUI.cs) a un GameObject (por ejemplo `UIManager` o `GameManager`).
   - Asigna el `EndGamePanel` al campo `endPanel`, y las referencias de `restartButton` y `quitButton` a los botones correspondientes.
   - Asegúrate que `EndGamePanel` esté desactivado (inactive) en el inicio de la escena.
   - Cuando el timer llegue a 0 el panel se activará automáticamente, detendrá el tiempo (Time.timeScale = 0) y mostrará los botones.
   - "Reiniciar" recarga la escena activa y "Salir" cierra la aplicación (en el editor detiene la reproducción).

Notas y ajustes
   - Ajusta `Ball.minImpactSpeed` para calibrar la fuerza requerida para contar como acierto.
   - Cambia `GameManager.timeRemaining` si quieres otro tiempo.
   - Puedes reproducir efectos o sonidos al `OnCollisionEnter` en `Ball` para dar feedback.
