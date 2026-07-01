#Requires -Version 5.1
<#
.SYNOPSIS
    Gera o icone do plugin ZimerfeldLFS.

    Layout: a area e' dividida em 4 partes iguais (quadrantes). Cada quadrante mostra um
    "file card" com um icone diferente de arquivo grande:
      • superior-esquerdo : joystick de video game
      • superior-direito  : violao/guitarra com notas musicais
      • inferior-esquerdo : botao de play (filme/video)
      • inferior-direito  : armazenamento de banco de dados (cilindros)
    No centro, uma pequena bomba com pavio aceso (estilo Super Mario Bros).

    Produz dois PNGs (fundo transparente):
      Resources\icon-128.png  (128x128 - icone do pacote nuget e da janela)
      Resources\ico.png       (16x16   - icone do menu Plugins)

    Desenho 100% GDI+/System.Drawing, deterministico, sem dependencias externas.
.NOTES
    Licensed under CC BY-NC-ND 4.0 - Copyright (c) 2026 Zimerfeld
#>
[CmdletBinding()]
param(
    [string]$OutDir = (Resolve-Path "$PSScriptRoot\..\..\src\GitExtensions.ZimerfeldLFS\Resources")
)

Add-Type -AssemblyName System.Drawing

function New-LfsIcon {
    param([int]$Size)

    $bmp = New-Object System.Drawing.Bitmap($Size, $Size, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode     = 'AntiAlias'
    $g.InterpolationMode = 'HighQualityBicubic'
    $g.PixelOffsetMode   = 'HighQuality'
    $g.Clear([System.Drawing.Color]::Transparent)

    $s = $Size / 128.0   # fator de escala (desenho pensado em 128px)
    $q = $Size / 4.0     # quarto da area

    # Cores por tipo (tint do card) e um pastel claro para o fundo do quadrante.
    $cJoy = [System.Drawing.Color]::FromArgb(255, 33, 150, 243)   # azul  - joystick
    $cAud = [System.Drawing.Color]::FromArgb(255, 156, 39, 176)   # roxo  - audio (nota musical)
    $cMov = [System.Drawing.Color]::FromArgb(255, 244, 67, 54)    # vermelho - play/filme
    $cDb  = [System.Drawing.Color]::FromArgb(255, 0, 150, 136)    # verde-azulado - database

    # ---- Fundo dos 4 quadrantes (pastel) + cruz divisoria -----------------------
    function Fill-Quadrant([double]$x, [double]$y, [System.Drawing.Color]$c) {
        $b = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(38, $c.R, $c.G, $c.B))
        $g.FillRectangle($b, [float]$x, [float]$y, [float]($Size/2), [float]($Size/2))
        $b.Dispose()
    }
    Fill-Quadrant 0            0            $cJoy
    Fill-Quadrant ($Size/2)    0            $cAud
    Fill-Quadrant 0            ($Size/2)    $cMov
    Fill-Quadrant ($Size/2)    ($Size/2)    $cDb

    $divPen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(120, 120, 120, 130), [float](1.5*$s))
    $g.DrawLine($divPen, [float]($Size/2), 0, [float]($Size/2), [float]$Size)
    $g.DrawLine($divPen, 0, [float]($Size/2), [float]$Size, [float]($Size/2))
    $divPen.Dispose()

    # ---- File card generico -----------------------------------------------------
    function Draw-FileCard {
        param(
            [double]$CenterX, [double]$CenterY, [double]$WidthU, [double]$HeightU,
            [System.Drawing.Color]$Tint, [string]$Kind)

        $w = $WidthU * $s
        $h = $HeightU * $s
        $state = $g.Save()
        $g.TranslateTransform([float]$CenterX, [float]$CenterY)
        $g.TranslateTransform([float](-$w/2), [float](-$h/2))

        $fold = 9 * $s
        $body = New-Object System.Drawing.Drawing2D.GraphicsPath
        $body.AddLine([float]0, [float]0, [float]($w - $fold), [float]0)
        $body.AddLine([float]($w - $fold), [float]0, [float]$w, [float]$fold)
        $body.AddLine([float]$w, [float]$fold, [float]$w, [float]$h)
        $body.AddLine([float]$w, [float]$h, [float]0, [float]$h)
        $body.CloseFigure()

        # Sombra
        $shadow = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(55, 0, 0, 0))
        $st2 = $g.Save(); $g.TranslateTransform([float](2*$s), [float](2*$s))
        $g.FillPath($shadow, $body); $g.Restore($st2)

        # Papel branco + contorno colorido
        $paper = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 250, 250, 252))
        $g.FillPath($paper, $body)
        $outline = New-Object System.Drawing.Pen($Tint, [float]([Math]::Max(1.0, 2.2*$s)))
        $g.DrawPath($outline, $body)

        # Ponta dobrada
        $foldBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 210, 210, 220))
        $foldPath  = New-Object System.Drawing.Drawing2D.GraphicsPath
        $foldPath.AddLine([float]($w - $fold), [float]0, [float]($w - $fold), [float]$fold)
        $foldPath.AddLine([float]($w - $fold), [float]$fold, [float]$w, [float]$fold)
        $foldPath.CloseFigure()
        $g.FillPath($foldBrush, $foldPath)

        # Faixa colorida na base
        $bandH = 11 * $s
        $band = New-Object System.Drawing.SolidBrush($Tint)
        $g.FillRectangle($band, [float]0, [float]($h - $bandH), [float]$w, [float]$bandH)

        # ---- Glifo por tipo ----
        $gx = $w / 2
        $gy = ($h - $bandH) / 2 + 2*$s
        $ink = New-Object System.Drawing.SolidBrush($Tint)
        $pen = New-Object System.Drawing.Pen($Tint, [float]([Math]::Max(1.2, 2.4*$s)))
        $pen.StartCap = 'Round'; $pen.EndCap = 'Round'; $pen.LineJoin = 'Round'

        switch ($Kind) {
            'joystick' {
                # Joystick de arcade completo: base + capa (dust cover) + haste + bola com brilho.
                # Placa de base (elipse escura) + tampo (elipse tint)
                $baseDark = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 55, 60, 70))
                $g.FillEllipse($baseDark, [float]($gx - 14*$s), [float]($gy + 9*$s), [float](28*$s), [float](8*$s))
                $baseDark.Dispose()
                $g.FillEllipse($ink, [float]($gx - 13*$s), [float]($gy + 7*$s), [float](26*$s), [float](7*$s))
                # Capa flexível (trapézio) subindo da base
                $boot = New-Object 'System.Drawing.PointF[]' 4
                $boot[0] = New-Object System.Drawing.PointF([float]($gx - 9*$s),  [float]($gy + 11*$s))
                $boot[1] = New-Object System.Drawing.PointF([float]($gx + 9*$s),  [float]($gy + 11*$s))
                $boot[2] = New-Object System.Drawing.PointF([float]($gx + 4.5*$s),[float]($gy - 1*$s))
                $boot[3] = New-Object System.Drawing.PointF([float]($gx - 4.5*$s),[float]($gy - 1*$s))
                $g.FillPolygon($ink, $boot)
                # Haste (cinza) da capa até a bola
                $shaft = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 90, 96, 108), [float](3.4*$s))
                $shaft.StartCap='Round'; $shaft.EndCap='Round'
                $g.DrawLine($shaft, [float]$gx, [float]($gy - 1*$s), [float]$gx, [float]($gy - 9*$s))
                $shaft.Dispose()
                # Bola vermelha com contorno e brilho
                $ball = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 229, 57, 53))
                $g.FillEllipse($ball, [float]($gx - 7*$s), [float]($gy - 16*$s), [float](14*$s), [float](14*$s))
                $ballPen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 150, 20, 20), [float](1.4*$s))
                $g.DrawEllipse($ballPen, [float]($gx - 7*$s), [float]($gy - 16*$s), [float](14*$s), [float](14*$s))
                $ballPen.Dispose(); $ball.Dispose()
                $gloss = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(200, 255, 255, 255))
                $g.FillEllipse($gloss, [float]($gx - 4*$s), [float]($gy - 14*$s), [float](5*$s), [float](4*$s))
                $gloss.Dispose()
            }
            'audio' {
                # Nota musical complexa: duas colcheias unidas por feixe duplo (semicolcheias).
                $xL = $gx - 2.5*$s; $xR = $gx + 9.5*$s
                # Cabeças de nota (elipses levemente inclinadas)
                $g.FillEllipse($ink, [float]($gx - 12*$s), [float]($gy + 6*$s), [float](10*$s), [float](7.5*$s))
                $g.FillEllipse($ink, [float]($gx),         [float]($gy + 8*$s), [float](10*$s), [float](7.5*$s))
                # Hastes
                $stem = New-Object System.Drawing.Pen($Tint, [float](2.4*$s)); $stem.StartCap='Round'; $stem.EndCap='Square'
                $g.DrawLine($stem, [float]$xL, [float]($gy + 8*$s),  [float]$xL, [float]($gy - 12*$s))
                $g.DrawLine($stem, [float]$xR, [float]($gy + 10*$s), [float]$xR, [float]($gy - 10*$s))
                $stem.Dispose()
                # Feixe duplo (dois paralelogramos inclinados unindo as hastes)
                function Beam([double]$yL, [double]$yR, [double]$t) {
                    $p = New-Object 'System.Drawing.PointF[]' 4
                    $p[0] = New-Object System.Drawing.PointF([float]$xL, [float]$yL)
                    $p[1] = New-Object System.Drawing.PointF([float]$xR, [float]$yR)
                    $p[2] = New-Object System.Drawing.PointF([float]$xR, [float]($yR + $t))
                    $p[3] = New-Object System.Drawing.PointF([float]$xL, [float]($yL + $t))
                    $g.FillPolygon($ink, $p)
                }
                Beam ($gy - 12*$s) ($gy - 10*$s) (3.4*$s)
                Beam ($gy - 6.5*$s) ($gy - 4.5*$s) (3.4*$s)
            }
            'movie' {
                # "Tela" arredondada + triangulo de play branco
                $screen = New-Object System.Drawing.Drawing2D.GraphicsPath
                $rr = 5*$s; $rx = $gx - 13*$s; $ry = $gy - 11*$s; $rw = 26*$s; $rh = 22*$s
                $screen.AddArc([float]$rx,           [float]$ry,           [float](2*$rr), [float](2*$rr), 180, 90)
                $screen.AddArc([float]($rx+$rw-2*$rr),[float]$ry,           [float](2*$rr), [float](2*$rr), 270, 90)
                $screen.AddArc([float]($rx+$rw-2*$rr),[float]($ry+$rh-2*$rr),[float](2*$rr),[float](2*$rr), 0,   90)
                $screen.AddArc([float]$rx,           [float]($ry+$rh-2*$rr),[float](2*$rr),[float](2*$rr), 90,  90)
                $screen.CloseFigure()
                $g.FillPath($ink, $screen)
                $tri = New-Object 'System.Drawing.PointF[]' 3
                $tri[0] = New-Object System.Drawing.PointF([float]($gx - 5*$s), [float]($gy - 7*$s))
                $tri[1] = New-Object System.Drawing.PointF([float]($gx - 5*$s), [float]($gy + 7*$s))
                $tri[2] = New-Object System.Drawing.PointF([float]($gx + 8*$s), [float]($gy))
                $white = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 250, 250, 252))
                $g.FillPolygon($white, $tri); $white.Dispose()
            }
            'database' {
                # Cilindros empilhados (armazenamento)
                $dw = 22*$s; $eh = 6*$s; $dh = 24*$s
                $left = $gx - $dw/2; $top = $gy - 12*$s
                $g.FillRectangle($ink, [float]$left, [float]($top + $eh/2), [float]$dw, [float]($dh - $eh))
                $g.FillEllipse($ink, [float]$left, [float]($top + $dh - $eh), [float]$dw, [float]$eh)
                $g.FillEllipse($ink, [float]$left, [float]$top, [float]$dw, [float]$eh)
                $lp = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 250, 250, 252), [float](1.4*$s))
                $g.DrawArc($lp, [float]$left, [float]($top + $dh/3.0), [float]$dw, [float]$eh, 0, 180)
                $g.DrawArc($lp, [float]$left, [float]($top + 2*$dh/3.0), [float]$dw, [float]$eh, 0, 180)
                $lp.Dispose()
            }
        }

        $g.Restore($state)
    }

    # ---- Um card por quadrante --------------------------------------------------
    $cw = 44; $ch = 54
    Draw-FileCard -CenterX $q          -CenterY $q          -WidthU $cw -HeightU $ch -Tint $cJoy -Kind 'joystick'
    Draw-FileCard -CenterX (3*$q)      -CenterY $q          -WidthU $cw -HeightU $ch -Tint $cAud -Kind 'audio'
    Draw-FileCard -CenterX $q          -CenterY (3*$q)      -WidthU $cw -HeightU $ch -Tint $cMov -Kind 'movie'
    Draw-FileCard -CenterX (3*$q)      -CenterY (3*$q)      -WidthU $cw -HeightU $ch -Tint $cDb  -Kind 'database'

    # ---- Bomba com pavio aceso no centro (estilo Mario Bros) --------------------
    $cx = $Size/2.0; $cy = $Size/2.0
    $r  = 17*$s

    # Halo claro para destacar a bomba do fundo dos quadrantes
    $halo = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(160, 255, 255, 255))
    $g.FillEllipse($halo, [float]($cx - $r - 4*$s), [float]($cy - $r - 4*$s), [float](2*$r + 8*$s), [float](2*$r + 8*$s))
    $halo.Dispose()

    # Esfera preta (gradiente radial para brilho)
    $bombPath = New-Object System.Drawing.Drawing2D.GraphicsPath
    $bombPath.AddEllipse([float]($cx - $r), [float]($cy - $r), [float](2*$r), [float](2*$r))
    $pgb = New-Object System.Drawing.Drawing2D.PathGradientBrush($bombPath)
    $pgb.CenterPoint    = New-Object System.Drawing.PointF([float]($cx - $r*0.35), [float]($cy - $r*0.35))
    $pgb.CenterColor    = [System.Drawing.Color]::FromArgb(255, 90, 90, 100)
    $pgb.SurroundColors = @([System.Drawing.Color]::FromArgb(255, 18, 18, 22))
    $g.FillPath($pgb, $bombPath)
    $bombOutline = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 5, 5, 8), [float](1.5*$s))
    $g.DrawPath($bombOutline, $bombPath)
    # Brilho especular
    $spec = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(200, 255, 255, 255))
    $g.FillEllipse($spec, [float]($cx - $r*0.55), [float]($cy - $r*0.55), [float]($r*0.5), [float]($r*0.38))
    $spec.Dispose()

    # Bocal/tampa no topo
    $capW = 10*$s; $capH = 7*$s
    $cap = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 60, 45, 30))
    $g.FillRectangle($cap, [float]($cx - $capW/2), [float]($cy - $r - $capH + 2*$s), [float]$capW, [float]$capH)
    $cap.Dispose()

    # Pavio curvo (bezier) subindo para a direita
    $fuse = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 120, 85, 45), [float](2.6*$s))
    $fuse.StartCap='Round'; $fuse.EndCap='Round'
    $p0 = New-Object System.Drawing.PointF([float]$cx,            [float]($cy - $r - $capH + 3*$s))
    $p1 = New-Object System.Drawing.PointF([float]($cx + 10*$s),  [float]($cy - $r - 14*$s))
    $p2 = New-Object System.Drawing.PointF([float]($cx - 2*$s),   [float]($cy - $r - 22*$s))
    $p3 = New-Object System.Drawing.PointF([float]($cx + 11*$s),  [float]($cy - $r - 28*$s))
    $g.DrawBezier($fuse, $p0, $p1, $p2, $p3)
    $fuse.Dispose()

    # Faisca (starburst) na ponta do pavio
    $fx = $p3.X; $fy = $p3.Y
    $sparkPts = New-Object 'System.Drawing.PointF[]' 16
    for ($i = 0; $i -lt 16; $i++) {
        $ang = [Math]::PI * $i / 8
        $rr  = if ($i % 2 -eq 0) { 9*$s } else { 4*$s }
        $sparkPts[$i] = New-Object System.Drawing.PointF([float]($fx + $rr*[Math]::Cos($ang)), [float]($fy + $rr*[Math]::Sin($ang)))
    }
    $sparkBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
        (New-Object System.Drawing.Point([int]($fx-9*$s),[int]($fy-9*$s))),
        (New-Object System.Drawing.Point([int]($fx+9*$s),[int]($fy+9*$s))),
        [System.Drawing.Color]::FromArgb(255, 255, 235, 59),
        [System.Drawing.Color]::FromArgb(255, 255, 87, 34))
    $g.FillPolygon($sparkBrush, $sparkPts)
    $sparkBrush.Dispose()
    $core = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 255, 253, 231))
    $g.FillEllipse($core, [float]($fx - 2.5*$s), [float]($fy - 2.5*$s), [float](5*$s), [float](5*$s))
    $core.Dispose()

    $g.Dispose()
    return $bmp
}

if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Force $OutDir | Out-Null }

$big = New-LfsIcon -Size 128
$big.Save((Join-Path $OutDir 'icon-128.png'), [System.Drawing.Imaging.ImageFormat]::Png)
Write-Host "icon-128.png gerado." -ForegroundColor Green

# 16x16: renderiza em 64 e reduz para nitidez.
$hi = New-LfsIcon -Size 64
$small = New-Object System.Drawing.Bitmap(16, 16, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$gs = [System.Drawing.Graphics]::FromImage($small)
$gs.SmoothingMode = 'AntiAlias'; $gs.InterpolationMode = 'HighQualityBicubic'; $gs.PixelOffsetMode = 'HighQuality'
$gs.Clear([System.Drawing.Color]::Transparent)
$gs.DrawImage($hi, (New-Object System.Drawing.Rectangle(0,0,16,16)))
$gs.Dispose()
$small.Save((Join-Path $OutDir 'ico.png'), [System.Drawing.Imaging.ImageFormat]::Png)
Write-Host "ico.png gerado." -ForegroundColor Green

$big.Dispose(); $hi.Dispose(); $small.Dispose()
Write-Host "Icones salvos em: $OutDir" -ForegroundColor Cyan
