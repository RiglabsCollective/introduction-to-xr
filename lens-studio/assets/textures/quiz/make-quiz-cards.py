"""
Makes the picture cards for the AR Quiz Lens (lens 006).

Every question, answer and message becomes a PNG with a rounded, coloured
card and bold rounded lettering, so the quiz can show stylish pictures
instead of plain 3D text.

Run it from this folder:   python make-quiz-cards.py
Change QUESTIONS below to make cards for your own quiz, then run it again.
Needs Python 3 and Pillow (pip install pillow).

The font is Arial Rounded MT Bold. If you don't have it, pass another font
file:   python make-quiz-cards.py --font path/to/font.ttf
"""
import argparse
import os
import re
from PIL import Image, ImageDraw, ImageFont

# (question, left answer, right answer, correct side)
QUESTIONS = [
    ("What does AR stand for?", "Augmented Reality", "Artificial Reality", "left"),
    ("What unit does Lens Studio use?", "Metre", "Centimetre", "right"),
    ("Which tracking puts objects on a table?", "Face tracking", "Surface tracking", "right"),
    ("What does a marker tracker look for?", "A known picture", "Your hand", "left"),
    ("Which axis points up?", "Y", "Z", "left"),
]

NAVY = (28, 32, 74)
CYAN = (90, 220, 255)
CORAL = (255, 99, 110)
TEAL = (24, 188, 156)
GOLD = (255, 200, 60)
PURPLE = (122, 92, 255)
GREEN = (46, 204, 113)
RED = (231, 76, 60)
WHITE = (255, 255, 255)

SCALE = 2  # draw big, then shrink for smooth edges


def find_font(path):
    for p in [path, "C:/Windows/Fonts/ARLRDBD.TTF", "/System/Library/Fonts/Supplemental/Arial Rounded Bold.ttf"]:
        if p and os.path.exists(p):
            return p
    raise SystemExit("Font not found. Pass one with --font path/to/font.ttf")


def wrap(draw, text, font, max_width):
    words, lines, line = text.split(), [], ""
    for w in words:
        trial = (line + " " + w).strip()
        if draw.textlength(trial, font=font) <= max_width or not line:
            line = trial
        else:
            lines.append(line)
            line = w
    lines.append(line)
    return lines


def card(text, size, fill, border, text_color=WHITE, font_path=None, font_size=64, arrow=None):
    w, h = size[0] * SCALE, size[1] * SCALE
    img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    r = h // 2 if h < 260 * SCALE else 48 * SCALE
    m = 6 * SCALE
    d.rounded_rectangle((m, m, w - m, h - m), r, fill=fill, outline=border, width=8 * SCALE)
    # soft highlight along the top
    shine = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    ImageDraw.Draw(shine).rounded_rectangle((m + 14 * SCALE, m + 10 * SCALE, w - m - 14 * SCALE, h * 0.42),
                                            r // 2, fill=(255, 255, 255, 38))
    img = Image.alpha_composite(img, shine)
    d = ImageDraw.Draw(img)
    pad = 78 * SCALE if arrow else 44 * SCALE
    font = ImageFont.truetype(font_path, font_size * SCALE)
    lines = wrap(d, text, font, w - 2 * pad)
    while len(lines) * font.size * 1.15 > h - 40 * SCALE and font.size > 20 * SCALE:
        font = ImageFont.truetype(font_path, int(font.size * 0.9))
        lines = wrap(d, text, font, w - 2 * pad)
    total = len(lines) * font.size * 1.15
    y = (h - total) / 2
    for line in lines:
        tw = d.textlength(line, font=font)
        d.text(((w - tw) / 2, y), line, font=font, fill=text_color,
               stroke_width=2 * SCALE, stroke_fill=(0, 0, 0, 90))
        y += font.size * 1.15
    if arrow:
        cy, s = h // 2, 22 * SCALE
        x = 44 * SCALE if arrow == "left" else w - 44 * SCALE
        dx = -s if arrow == "left" else s
        d.polygon([(x - dx, cy - s), (x - dx, cy + s), (x + dx, cy)], fill=text_color)
    return img.resize(size, Image.LANCZOS)


def slug(text):
    return re.sub(r"[^a-z0-9]+", "-", text.lower()).strip("-")[:40]


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--font", default=None)
    font = find_font(ap.parse_args().font)
    here = os.path.dirname(os.path.abspath(__file__))

    for i, (q, left, right, correct) in enumerate(QUESTIONS, 1):
        n = "%02d" % i
        card(q, (1024, 280), NAVY, CYAN, font_path=font, font_size=72).save(os.path.join(here, "question-%s.png" % n))
        card(left, (680, 170), CORAL, WHITE, font_path=font, font_size=58, arrow="left").save(os.path.join(here, "left-%s.png" % n))
        card(right, (680, 170), TEAL, WHITE, font_path=font, font_size=58, arrow="right").save(os.path.join(here, "right-%s.png" % n))

    card("Brows up = left   Mouth open = right", (1100, 160), PURPLE, WHITE, font_path=font, font_size=54).save(os.path.join(here, "message-hint.png"))
    card("Correct!", (640, 170), GREEN, WHITE, font_path=font, font_size=84).save(os.path.join(here, "message-correct.png"))
    card("Not quite", (640, 170), RED, WHITE, font_path=font, font_size=84).save(os.path.join(here, "message-wrong.png"))
    card("Quiz complete!", (760, 220), GOLD, WHITE, text_color=NAVY, font_path=font, font_size=96).save(os.path.join(here, "message-done.png"))
    print("Made", len(os.listdir(here)) - 1, "pictures in", here)


if __name__ == "__main__":
    main()
