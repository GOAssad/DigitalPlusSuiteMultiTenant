<?php
header('Content-Type: application/json');

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    http_response_code(405);
    echo json_encode(['ok' => false, 'error' => 'Método no permitido']);
    exit;
}

$nombre   = htmlspecialchars(trim($_POST['nombre'] ?? ''), ENT_QUOTES, 'UTF-8');
$empresa  = htmlspecialchars(trim($_POST['empresa'] ?? ''), ENT_QUOTES, 'UTF-8');
$email    = filter_var(trim($_POST['email'] ?? ''), FILTER_VALIDATE_EMAIL);
$servicio = htmlspecialchars(trim($_POST['servicio'] ?? ''), ENT_QUOTES, 'UTF-8');
$mensaje  = htmlspecialchars(trim($_POST['mensaje'] ?? ''), ENT_QUOTES, 'UTF-8');

if (!$nombre || !$email) {
    http_response_code(400);
    echo json_encode(['ok' => false, 'error' => 'Faltan campos obligatorios']);
    exit;
}

$destinatario = 'administracion@integraai.com.ar';
$asunto       = "Nueva consulta web — $nombre" . ($empresa ? " ($empresa)" : '');

$cuerpo  = "Nombre: $nombre\n";
$cuerpo .= "Empresa: $empresa\n";
$cuerpo .= "Email: $email\n";
$cuerpo .= "Servicio: $servicio\n";
$cuerpo .= "Mensaje:\n$mensaje\n";

$headers  = "From: no-reply@integraia.tech\r\n";
$headers .= "Reply-To: $email\r\n";
$headers .= "Content-Type: text/plain; charset=UTF-8\r\n";

$enviado = mail($destinatario, $asunto, $cuerpo, $headers);

if ($enviado) {
    echo json_encode(['ok' => true]);
} else {
    http_response_code(500);
    echo json_encode(['ok' => false, 'error' => 'No se pudo enviar el correo']);
}
