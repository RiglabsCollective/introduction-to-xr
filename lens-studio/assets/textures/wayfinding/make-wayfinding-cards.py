"""
Makes the picture cards for the Campus Wayfinding lens (lens 010).

For every place in PLACES it makes:
  sign-<name>.png   the direction sign on the post (arrow, name and distance)
  info-<name>.png   the information card that opens when you tap the sign
and one chevron, ground-arrow.png, for the path on the floor.

Run it from this folder:   python make-wayfinding-cards.py
Change PLACES below to make cards for your own campus, then run it again.
Needs Python 3 and Pillow (pip install pillow).
The font is Arial Rounded MT Bold. Use --font path/to/font.ttf for another.
"""
import argparse
import os
from PIL import Image, ImageDraw, ImageFont

# name, direction (left, right or ahead), distance, building line, hours line
PLACES = [
    ("computer-lab", "left", "40 m", "Computer Lab", "Block B, Floor 2", "Open 9:00 to 18:00"),
    ("library", "right", "120 m", "Library", "Main Building, Floor 1", "Open 8:00 to 20:00"),
    ("cafeteria", "ahead", "80 m", "Cafeteria", "Student Centre, Ground floor", "Open 7:30 to 17:00"),
]
COLORS = {"left": (20, 140, 110), "right": (45, 100, 190), "ahead": (230, 130, 30)}
NAVY = (22, 28, 70)
WHITE = (255, 255, 255)
SCALE = 2


def find_font(path):
    for p in [path, "C:/Windows/Fonts/ARLRDBD.TTF", "/System/Library/Fonts/Supplemental/Arial Rounded Bold.ttf"]:
        if p and os.path.exists(p):
            return p
    raise SystemExit("Font not found. Pass one with --font path/to/font.ttf")


def rounded(size, fill, border, radius):
    w, h = size[0] * SCALE, size[1] * SCALE
    img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    m = 6 * SCALE
    d.rounded_rectangle((m, m, w - m, h - m), radius * SCALE, fill=fill, outline=border, width=8 * SCALE)
    shine = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    ImageDraw.Draw(shine).rounded_rectangle((m + 14 * SCALE, m + 10 * SCALE, w - m - 14 * SCALE, h * 0.4),
                                            (radius // 2) * SCALE, fill=(255, 255, 255, 36))
    return Image.alpha_composite(img, shine)


def text(img, xy, s, font, fill=WHITE, anchor="la"):
    d = ImageDraw.Draw(img)
    d.text(xy, s, font=font, fill=fill, anchor=anchor, stroke_width=2 * SCALE, stroke_fill=(0, 0, 0, 80))


def arrow(img, cx, cy, direction, size, fill=WHITE):
    d = ImageDraw.Draw(img)
    s = size * SCALE
    if direction == "left":
        pts = [(cx - s, cy), (cx, cy - s), (cx, cy - s * 0.4), (cx + s, cy - s * 0.4), (cx + s, cy + s * 0.4), (cx, cy + s * 0.4), (cx, cy + s)]
    elif direction == "right":
        pts = [(cx + s, cy), (cx, cy - s), (cx, cy - s * 0.4), (cx - s, cy - s * 0.4), (cx - s, cy + s * 0.4), (cx, cy + s * 0.4), (cx, cy + s)]
    else:  # ahead (up)
        pts = [(cx, cy - s), (cx - s, cy), (cx - s * 0.4, cy), (cx - s * 0.4, cy + s), (cx + s * 0.4, cy + s), (cx + s * 0.4, cy), (cx + s, cy)]
    d.polygon(pts, fill=fill)


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--font", default=None)
    font = find_font(ap.parse_args().font)
    here = os.path.dirname(os.path.abspath(__file__))
    big = ImageFont.truetype(font, 78 * SCALE)
    mid = ImageFont.truetype(font, 52 * SCALE)
    small = ImageFont.truetype(font, 44 * SCALE)

    for name, direction, dist, title, building, hours in PLACES:
        # Sign on the post: 800 x 220
        W, H = 800, 220
        sign = rounded((W, H), COLORS[direction], WHITE, 60)
        ax = 110 * SCALE if direction != "right" else (W - 110) * SCALE
        arrow(sign, ax, (H // 2) * SCALE, direction, 55)
        tx = (W // 2 + (60 if direction != "right" else -60)) * SCALE
        text(sign, (tx, (H // 2 - 28) * SCALE), title, big, anchor="mm")
        text(sign, (tx, (H // 2 + 52) * SCALE), dist, mid, anchor="mm")
        sign.resize((W, H), Image.LANCZOS).save(os.path.join(here, "sign-%s.png" % name))

        # Info card: 900 x 420
        W, H = 900, 420
        info = rounded((W, H), NAVY, COLORS[direction], 50)
        text(info, (W // 2 * SCALE, 110 * SCALE), title, big, anchor="mm")
        text(info, (W // 2 * SCALE, 230 * SCALE), building, mid, anchor="mm")
        text(info, (W // 2 * SCALE, 320 * SCALE), hours, small, fill=(255, 215, 120), anchor="mm")
        info.resize((W, H), Image.LANCZOS).save(os.path.join(here, "info-%s.png" % name))

    # Ground chevron: 512 x 512, points up
    g = Image.new("RGBA", (512 * SCALE, 512 * SCALE), (0, 0, 0, 0))
    d = ImageDraw.Draw(g)
    s = SCALE
    d.polygon([(256 * s, 40 * s), (470 * s, 300 * s), (380 * s, 300 * s), (256 * s, 150 * s), (132 * s, 300 * s), (42 * s, 300 * s)], fill=(255, 215, 60, 255), outline=WHITE)
    d.polygon([(256 * s, 200 * s), (470 * s, 460 * s), (380 * s, 460 * s), (256 * s, 310 * s), (132 * s, 460 * s), (42 * s, 460 * s)], fill=(255, 215, 60, 170))
    g.resize((512, 512), Image.LANCZOS).save(os.path.join(here, "ground-arrow.png"))
    print("Made", len(os.listdir(here)) - 1, "pictures in", here)


if __name__ == "__main__":
    main()
