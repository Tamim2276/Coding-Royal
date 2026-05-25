import sys
from pypdf import PdfReader

def extract_pdf():
    try:
        reader = PdfReader("C:/Users/tam1m/Desktop/coding game/last_draft_for_code_Royal.pdf")
        text = ""
        for page in reader.pages:
            extracted = page.extract_text()
            if extracted:
                text += extracted + "\n\n"
        
        with open("C:/Users/tam1m/Desktop/coding game/draft_text.txt", "w", encoding="utf-8") as f:
            f.write(text)
        print("Success: draft_text.txt created.")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    extract_pdf()