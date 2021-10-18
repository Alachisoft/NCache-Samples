package com.alachisoft.ncache.springbootsample.bookstore.controller;

import com.alachisoft.ncache.springbootsample.bookstore.Book;
import com.alachisoft.ncache.springbootsample.bookstore.service.BookService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.propertyeditors.CustomDateEditor;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.ui.ModelMap;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.WebDataBinder;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.view.RedirectView;

import java.text.SimpleDateFormat;
import java.util.Date;

@Controller
public class BookController {

    @Autowired
    BookService booksService;

    @InitBinder
    public void initBinder(WebDataBinder binder) {
        SimpleDateFormat sdf = new SimpleDateFormat("MM-dd-yyyy");
        sdf.setLenient(true);
        binder.registerCustomEditor(Date.class, new CustomDateEditor(sdf, true));
    }

    @RequestMapping(value="/bookstore", method = RequestMethod.GET)
    public ModelAndView homePage(Model model){
        model.addAttribute("availableBooks", booksService.listAll());
        return new ModelAndView("bookstore");
    }

    @RequestMapping(value = "/bookstore/new", method = RequestMethod.GET)
    public String add(ModelMap model) {
        model.addAttribute("book", new Book());
        return "newBook";
    }

    @RequestMapping(value = "/bookstore/save", method = RequestMethod.POST)
    public String saveBook(@ModelAttribute("book") Book book, BindingResult result) {
        if (result.hasErrors()) {
            return "newBook";
        }
        booksService.save(book);
        return "redirect:/bookstore";
    }

    @RequestMapping(value = "/bookstore/edit", method = RequestMethod.GET)
    public String showEditBookPage(@RequestParam long id, ModelMap model) {
        Book book = booksService.get(id);
        model.addAttribute("book", book == null ? new Book() : book);
        return "newBook";
    }

    @RequestMapping(value = "/bookstore/edit", method = RequestMethod.POST)
    public String updateBook(@ModelAttribute("book") Book book, BindingResult result) {
        if (result.hasErrors()) {
            return "newBook";
        }
        booksService.update(book);
        return "redirect:/bookstore";
    }

    @RequestMapping(value = "/bookstore/delete")
    public String deleteBook(@RequestParam int id) {
        booksService.delete(id);
        return "redirect:/bookstore";
    }

    @RequestMapping(value="/bookstore", method = RequestMethod.POST)
    public ModelAndView findBook(ModelMap model, @RequestParam long isbn){

        Book book = booksService.get(isbn);

        if (book == null) {
            return returnError(model, isbn);
        }
        model.put("book", book);
        return new ModelAndView("bookdetails");
    }

    private ModelAndView returnError(ModelMap model, long isbn) {
        String errorMessage = "The book with ISBN: " + isbn + " is not available.";
        model.put("errorMessage", errorMessage);
        return new ModelAndView(new RedirectView("bookstore"));
    }
}