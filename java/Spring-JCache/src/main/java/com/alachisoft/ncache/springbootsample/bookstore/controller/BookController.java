package com.alachisoft.ncache.springbootsample.bookstore.controller;

import com.alachisoft.ncache.springbootsample.bookstore.Book;
import com.alachisoft.ncache.springbootsample.bookstore.service.BookService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.view.RedirectView;

import java.beans.BeanProperty;
import java.util.Map;

@Controller
public class BookController {

    @Autowired
    BookService booksService;

    @RequestMapping(value="/bookstore", method = RequestMethod.GET)
    public ModelAndView homePage(ModelMap model){
        model.put("availableBooks", booksService.getBookTitles());
        return new ModelAndView("bookstore");
    }

    @RequestMapping(value="/bookstore", method = RequestMethod.POST)
    public ModelAndView findBook(ModelMap model, @RequestParam long isbn){

        Book book = booksService.getBookByISBN(isbn);

        if (book == null) {
            String errorMessage = "The book with ISBN: " + isbn + " is not available.";
            model.put("errorMessage", errorMessage);
            return new ModelAndView(new RedirectView("bookstore"));
        }
        model.put("book", book);
        return new ModelAndView("bookdetails");
    }

    @GetMapping("/books")
    public Book findBook(@PathVariable long isbn) {
        return booksService.getBookByISBN(isbn);
    }
}